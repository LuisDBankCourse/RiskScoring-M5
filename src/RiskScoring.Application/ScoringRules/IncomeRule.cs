using RiskScoring.Application.Contracts;
using RiskScoring.Domain.ValueObjects;

namespace RiskScoring.Application.ScoringRules;

public class IncomeRule : IScoringRule
{
    public string RuleName => "IncomeRule";

    public int MaxPoint => 30;

    public int Evaluate(ScoringContext context)
    {
        return context.MonthlyIncome >= 5000 ? 30 : 0;
    }
}