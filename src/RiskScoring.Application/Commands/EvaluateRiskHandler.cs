using System.Text.Json;
using Customer.Application.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using RiskScoring.Application.Contracts;
using RiskScoring.Domain.Repository;
using RiskScoring.Domain.ValueObjects;

namespace RiskScoring.Application.Commands;

public class EvaluateRiskHandler : IRequestHandler<EvaluateRiskCommand, Result<EvaluateRiskResponse>>
    {
        private readonly IRiskEvaluationRepository _evaluationRepository;
        private readonly ILogger<EvaluateRiskHandler> _logger;

        private readonly IEnumerable<IScoringRule> _scoringRules;

        public EvaluateRiskHandler(
            IRiskEvaluationRepository evaluationRepository,
            ILogger<EvaluateRiskHandler> logger,
            IEnumerable<IScoringRule> scoringRules
            )
        {
            _evaluationRepository = evaluationRepository;
            _logger = logger;
            _scoringRules = scoringRules;
        }


        public async Task<Result<EvaluateRiskResponse>> Handle(EvaluateRiskCommand request, CancellationToken ct)
        {
            _logger.LogInformation(
                "Starting risk evaluation. ApplicationId={ApplicationId}, TermMonths={TermMonths}, CustomerId={CustomerId}, RequestedAmount={RequestedAmount}",
                request.ApplicationId,
                request.TermMonths,
                request.CustomerId,
                request.RequestedAmount);
            var context = new ScoringContext
            {
                ApplicationId = request.ApplicationId,
                CustomerId = request.CustomerId,
                MonthlyIncome = request.MonthlyIncome, // Nota: en tu código original tenías request.RequestedAmount asignado aquí
                EmploymentType = request.EmploymentType,
                RequestedAmount = request.RequestedAmount,
                TermMonths = request.TermMonths,
                MinAmount = request.MinAmount,
                MaxAmount = request.MaxAmount,
                MinTermMonths = request.MinTermMonths,
                MaxTermMonths = request.MaxTermMonths,
                MaxInstallmentIncomeRatio = request.MaxInstallmentIncomeRatio,
                CorrelationId = request.CorrelationId,
            };
            var productViolation = ValidateProductRules(context);
            if (productViolation is not null)
            {
                _logger.LogWarning(
                    "Product rule violation detected. Reason={Reason}, ApplicationId={ApplicationId}",
                    productViolation, request.ApplicationId);
                var evaluationRiskViolate = Domain.Entities.RiskEvaluation.Create(
                    request.ApplicationId,
                    request.CustomerId,
                    score: 0,
                    riskLevel: "High",
                    decision: "Rejected",
                    reason: productViolation,
                    evaluationDetails: "{}",
                    correlationId: request.CorrelationId
                );
                await _evaluationRepository.AddAsync(evaluationRiskViolate, ct);
                await _evaluationRepository.SaveChangesAsync(ct);
                return Result<EvaluateRiskResponse>.Success(new EvaluateRiskResponse(
                    Score: 0,
                    RiskLevel: "High",
                    Decision: "Rejected",
                    Reason: productViolation,
                    RuleBreakdown: new Dictionary<string, int>()
                ));
            }
            var breakdown = new Dictionary<string, int>();
            int totalScore = 0;
            foreach (var rule in _scoringRules)
            {
                int points = rule.Evaluate(context);
                breakdown[rule.RuleName] = points;
                totalScore += points;
                _logger.LogDebug(
                    "Rule evaluated. Rule={RuleName}, Points={Points}/{MaxPoint}",
                    rule.RuleName, points, rule.MaxPoint);
            }
            var (riskLevel, decision, reason) = DetermineDecision(totalScore);
            _logger.LogDebug(
                "Risk evaluation completed. Score={Score}, RiskLevel={RiskLevel}, Decision={Decision}, Reason={Reason}",
                totalScore, riskLevel, decision, reason);
            var evaluationDetails = JsonSerializer.Serialize(breakdown);
            var evaluationRisk = Domain.Entities.RiskEvaluation.Create(
                request.ApplicationId,
                request.CustomerId,
                totalScore,
                riskLevel,
                decision,
                reason,
                evaluationDetails,
                request.CorrelationId
            );
            await _evaluationRepository.AddAsync(evaluationRisk, ct);
            await _evaluationRepository.SaveChangesAsync(ct);
            return Result<EvaluateRiskResponse>.Success(new EvaluateRiskResponse(
                Score: totalScore,
                RiskLevel: riskLevel,
                Decision: decision,
                Reason: reason,
                RuleBreakdown: breakdown
            ));
        }
        private static string? ValidateProductRules(ScoringContext ctx)
        {
            if (ctx.RequestedAmount < ctx.MinAmount)
                return $"The requested amount {ctx.RequestedAmount} is less than the minimum allowed amount of {ctx.MinAmount} for this product.";
            if (ctx.RequestedAmount > ctx.MaxAmount)
                return $"The requested amount {ctx.RequestedAmount} exceeds the maximum allowed amount of {ctx.MaxAmount} for this product.";
            if (ctx.TermMonths < ctx.MinTermMonths)
                return $"The requested term of {ctx.TermMonths} months is less than the minimum allowed term of {ctx.MinTermMonths} months for this product.";
            if (ctx.TermMonths > ctx.MaxTermMonths)
                return $"The requested term of {ctx.TermMonths} months exceeds the maximum allowed term of {ctx.MaxTermMonths} months for this product.";
            return null;
        }
        private static (string riskLevel, string decision, string reason) DetermineDecision(int score)
        {
            return score switch
            {
                >= 85 => ("Low", "PreApproved", "Customer meets all product requirements and risk criteria"),
                >= 60 => ("Medium", "ManualReview", "Score requires manual review by an analyst"),
                _ => ("High", "Rejected", "Score is below the minimum threshold for approval"),
            };
        }
    }