using RiskScoring.Application.Contracts;
using RiskScoring.Domain.ValueObjects;

namespace RiskScoring.Application.ScoringRules;

public class InstallmentIncomeRatioRule : IScoringRule
{
    public string RuleName => "InstallmentIncomeRatioRule";

    public int MaxPoint => 20;

    public int Evaluate(ScoringContext context)
    {
        return context.InstallmentIncomeRatio <= context.MaxInstallmentIncomeRatio
            ? 20 : 0;
    }
}