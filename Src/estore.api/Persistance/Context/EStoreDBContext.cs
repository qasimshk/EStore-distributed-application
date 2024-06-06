namespace estore.api.Persistance.Context;

using estore.api.Common.Models;
using estore.api.Models.Aggregates;
using estore.api.Models.Aggregates.Customer;
using estore.api.Models.Aggregates.Employee;
using estore.api.Models.Aggregates.Orders;
using Microsoft.EntityFrameworkCore;

public class EStoreDBContext : DbContext, IUnitOfWork
{
    public EStoreDBContext(DbContextOptions<EStoreDBContext> options) : base(options) { }

    public DbSet<Order> Orders { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<Region> Regions { get; set; }

    public DbSet<Shipper> Shippers { get; set; }

    public DbSet<Supplier> Suppliers { get; set; }

    public DbSet<Territory> Territories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EStoreDBContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default) =>
        await base.SaveChangesAsync(cancellationToken);
}

// Add-Migration CreateDatabase -OutputDir "Persistance/Migrations"
// Update-Database
