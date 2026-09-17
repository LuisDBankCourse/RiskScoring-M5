namespace RiskScoring.Domain.ValueObjects;

public class ScoringContext
{
    public Guid ApplicationId { get; set; }
    public Guid CustomerId { get; set; }

    public decimal MonthlyIncome { get; set; }
    public string EmploymentType { get; set; }

    public decimal RequestedAmount { get; set; }
    public int TermMonths { get; set; }


    public decimal MinAmount { get; set; }
    public decimal MaxAmount { get; set; }
    public int MinTermMonths { get; set; }
    public int MaxTermMonths { get; set; }
    public decimal MaxInstallmentIncomeRatio { get; set; }

    public string CorrelationId { get; set; }

    public decimal EstimatedMonthlyInstallament =>
        TermMonths > 0 ? Math.Round(RequestedAmount / TermMonths, 2) : 0;

    public decimal InstallmentIncomeRatio =>
        MonthlyIncome > 0 ? Math.Round(EstimatedMonthlyInstallament / MonthlyIncome, 4) : decimal.MaxValue;



}