namespace RiskScoring.Domain.Entities;

public class RiskEvaluation
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid CustomerId { get; set; }
    public int Score { get; set; }
    public string RiskLevel { get; set; } = default!;
    public string Decision { get; set; } = default!;
    public string Reason { get; set; } = default!;


    public string EvaluationDetails { get; set; } = default!;

    public string CorrelationId { get; set; } = default!;
    public DateTime EvaluatedAtUtc { get; set; } = default!;

    public static RiskEvaluation Create(
        Guid applicationId,
        Guid customerId,
        int score,
        string riskLevel,
        string decision,
        string reason,
        string evaluationDetails,
        string correlationId
    )
    {

        ArgumentException.ThrowIfNullOrWhiteSpace(riskLevel);
        ArgumentException.ThrowIfNullOrWhiteSpace(decision);
        ArgumentException.ThrowIfNullOrWhiteSpace(reason);
        ArgumentException.ThrowIfNullOrWhiteSpace(evaluationDetails);
        ArgumentException.ThrowIfNullOrWhiteSpace(correlationId);

        return new RiskEvaluation
        {
            Id = Guid.NewGuid(),
            ApplicationId = applicationId,
            CustomerId = customerId,
            Score = score,
            RiskLevel = riskLevel,
            Decision = decision,
            Reason = reason,
            EvaluationDetails = evaluationDetails,
            CorrelationId = correlationId,
            EvaluatedAtUtc = DateTime.UtcNow
        };
    }



}