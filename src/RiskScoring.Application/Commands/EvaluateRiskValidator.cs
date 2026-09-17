using FluentValidation;

namespace RiskScoring.Application.Commands;

public class EvaluateRiskValidator : AbstractValidator<EvaluateRiskCommand>
{
    public EvaluateRiskValidator()
    {
        RuleFor(x => x.ApplicationId)
            .NotEmpty().WithMessage("ApplicationId is required");
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required");
        RuleFor(x => x.MonthlyIncome)
            .GreaterThan(0).WithMessage("MonthlyIncome must be greater than 0");
        RuleFor(x => x.EmploymentType)
            .NotEmpty().WithMessage("EmploymentType is required");
        RuleFor(x => x.RequestedAmount)
            .GreaterThan(0).WithMessage("RequestedAmount must be greater than 0");
        RuleFor(x => x.TermMonths)
            .GreaterThan(0).WithMessage("TermMonths must be greater than 0");
        RuleFor(x => x.MinAmount)
            .GreaterThan(0).WithMessage("MinAmount must be greater than 0");
        RuleFor(x => x.MaxAmount)
            .GreaterThan(x => x.MinAmount).WithMessage("MaxAmount must be greater than MinAmount");
        RuleFor(x => x.MinTermMonths)
            .GreaterThan(0).WithMessage("MinTermMonths must be greater than 0");
        RuleFor(x => x.MaxTermMonths)
            .GreaterThan(x => x.MinTermMonths).WithMessage("MaxTermMonths must be greater than MinTermMonths");
        RuleFor(x => x.MaxInstallmentIncomeRatio)
            .GreaterThan(0).LessThan(1).WithMessage("MaxInstallmentIncomeRatio must be greater than 0 and less than 1");
        RuleFor(x => x.CorrelationId)
            .NotEmpty().WithMessage("CorrelationId is required");
    }
}