using Microsoft.EntityFrameworkCore;

namespace RiskScoring.Infrastructure.Persistence;

public class RiskScoringDbContext : DbContext
{
    public RiskScoringDbContext(DbContextOptions<RiskScoringDbContext> options)
        : base(options)
    {

    }

    public DbSet<RiskScoring.Domain.Entities.RiskEvaluation> RiskEvaluations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.RiskEvaluation>(e =>
        {
            e.HasKey(x => x.Id);

            e.Property(x => x.RiskLevel)
                .HasMaxLength(20)
                .IsRequired();

            e.Property(x => x.Decision)
                .HasMaxLength(30)
                .IsRequired();

            e.Property(x => x.Reason)
                .HasMaxLength(500)
                .IsRequired();

            e.Property(x => x.EvaluationDetails)
                .IsRequired();

            e.Property(x => x.CorrelationId)
                .IsRequired();

            e.HasIndex(x => x.ApplicationId)
                .HasDatabaseName("Ix_ApplicationId");


            e.HasIndex(x => x.CustomerId)
                .HasDatabaseName("Ix_CustomerId");


            e.HasIndex(x => x.CorrelationId)
                .HasDatabaseName("Ix_CorrelationId");

        });


    }
}