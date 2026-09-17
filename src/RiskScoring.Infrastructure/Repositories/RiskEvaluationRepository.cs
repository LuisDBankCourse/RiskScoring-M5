using RiskScoring.Domain.Entities;
using RiskScoring.Domain.Repository;
using RiskScoring.Infrastructure.Persistence;

namespace RiskScoring.Infrastructure.Repositories;

public class RiskEvaluationRepository : IRiskEvaluationRepository
{
    private readonly RiskScoringDbContext _riskScoringDb;

    public RiskEvaluationRepository(RiskScoringDbContext riskScoringDb)
    {
        _riskScoringDb = riskScoringDb;
    }

    public async Task AddAsync(RiskEvaluation riskEvaluation, CancellationToken ct = default)
        => await _riskScoringDb.RiskEvaluations.AddAsync(riskEvaluation, ct);

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await _riskScoringDb.SaveChangesAsync(ct);
}