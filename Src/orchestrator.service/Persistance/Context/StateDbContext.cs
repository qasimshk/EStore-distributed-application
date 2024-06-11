namespace orchestrator.service.Persistance.Context;

using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using orchestrator.service.Persistance.Configurations;
using orchestrator.service.Persistance.Entities;

public class StateDbContext : SagaDbContext
{
    public StateDbContext(DbContextOptions<StateDbContext> options) : base(options) { }

    public DbSet<OrderState> Services { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StateDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { yield return new StateEntityTypeConfiguration(); }
    }
}

// Add-Migration CreateDatabase -OutputDir "Persistance/Migrations"
// Update-Database
