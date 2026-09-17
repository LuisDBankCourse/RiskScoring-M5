using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RiskScoring.Domain.Repository;
using RiskScoring.Infrastructure.Persistence;
using RiskScoring.Infrastructure.Repositories;

namespace RiskScoring.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RiskScoringDbContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("RiskScoringConnection"))
        );

        services.AddScoped<IRiskEvaluationRepository, RiskEvaluationRepository>();

        return services;

    }
}