using RiskScoring.Domain.Entities;

namespace RiskScoring.Domain.Repository;

public interface IRiskEvaluationRepository
{
    Task AddAsync(RiskEvaluation riskEvaluation, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);

}