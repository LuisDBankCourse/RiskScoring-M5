using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RiskScoring.Application.Commands;
using RiskScoring.Application.Contracts;
using RiskScoring.Application.ScoringRules;

namespace RiskScoring.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        services.AddValidatorsFromAssemblyContaining<EvaluateRiskValidator>();

        services.AddSingleton<IScoringRule, IncomeRule>();
        services.AddSingleton<IScoringRule, AmountToIncomeRule>();
        services.AddSingleton<IScoringRule, EmploymentTypeRule>();
        services.AddSingleton<IScoringRule, TermRule>();
        services.AddSingleton<IScoringRule, InstallmentIncomeRatioRule>();

        return services;
    }
}