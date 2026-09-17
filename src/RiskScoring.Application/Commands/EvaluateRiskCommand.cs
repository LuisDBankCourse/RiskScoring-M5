using Customer.Application.Common;
using MediatR;

namespace RiskScoring.Application.Commands;

public record EvaluateRiskCommand
(
    Guid ApplicationId,
    Guid CustomerId,
    decimal MonthlyIncome,
    string EmploymentType,
    decimal RequestedAmount,
    int TermMonths,
    decimal MinAmount,
    decimal MaxAmount,
    int MinTermMonths,
    int MaxTermMonths,
    decimal MaxInstallmentIncomeRatio,

    string CorrelationId

):IRequest<Result<EvaluateRiskResponse>>;


public record EvaluateRiskResponse
(
    int Score,
    string RiskLevel,
    string Decision,
    string Reason,
    Dictionary<string, int> RuleBreakdown

);