using RiskScoring.Application.Contracts;
using RiskScoring.Domain.ValueObjects;

namespace RiskScoring.Application.ScoringRules;

public class TermRule : IScoringRule
{
    public string RuleName => "TermRule";

    public int MaxPoint => 15;

    public int Evaluate(ScoringContext context)
    {
        return context.TermMonths switch
        {
            <= 24 => 25,
            <= 36 => 10,
            _ => 0
        };
    }
}