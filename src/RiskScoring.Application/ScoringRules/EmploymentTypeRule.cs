using RiskScoring.Application.Contracts;
using RiskScoring.Domain.ValueObjects;

namespace RiskScoring.Application.ScoringRules;

public class EmploymentTypeRule : IScoringRule
{
    public string RuleName => "EmploymentTypeRule";

    public int MaxPoint => 30;

    public int Evaluate(ScoringContext context)
    {

        return context.EmploymentType.ToUpperInvariant() switch
        {
            "PAYROLL" => 30,
            "INDEPENDENT" => 15,
            _ => 0

        };
    }
}