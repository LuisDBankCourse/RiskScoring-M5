using RiskScoring.Application.Contracts;
using RiskScoring.Domain.ValueObjects;

namespace RiskScoring.Application.ScoringRules;

public class AmountToIncomeRule : IScoringRule
{
    public string RuleName => "AmountToIncomeRule";

    public int MaxPoint => 25;

    public int Evaluate(ScoringContext context)
    {
        if (context.MonthlyIncome <= 0) return 0;

        var ratio = context.RequestedAmount / context.MonthlyIncome;

        return ratio switch
        {
            <= 3m => 25,
            <= 5m => 10,
            _ => 0
        };
    }
}