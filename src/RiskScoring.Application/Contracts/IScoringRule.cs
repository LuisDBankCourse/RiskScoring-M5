using RiskScoring.Domain.ValueObjects;

namespace RiskScoring.Application.Contracts;

public interface IScoringRule
{
    string RuleName { get; }
    int MaxPoint { get; }
    int Evaluate(ScoringContext context);
}