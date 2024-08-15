namespace orchestrator.api.Persistance.Context;

using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using orchestrator.api.Persistance.Configurations;
using orchestrator.api.Persistance.Entities;

public class StateDbContext(DbContextOptions<StateDbContext> options) : SagaDbContext(options)
{
    public DbSet<OrderState> OrderStates { get; set; }

    public DbSet<PaymentState> PaymentStates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StateDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get
        {
            yield return new OrderStateEntityTypeConfiguration();
            yield return new PaymentStateEntityTypeConfiguration();
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.ConfigureWarnings(w => w.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS));
}

// Add-Migration CreateDatabase -OutputDir "Persistance/Migrations"
// Update-Database
