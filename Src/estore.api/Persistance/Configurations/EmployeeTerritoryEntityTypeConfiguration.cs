namespace estore.api.Persistance.Configurations;

using estore.api.Models.Aggregates.Employee.Entities;
using estore.api.Models.Aggregates.Employee.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EmployeeTerritoryEntityTypeConfiguration : IEntityTypeConfiguration<EmployeeTerritory>
{
    public void Configure(EntityTypeBuilder<EmployeeTerritory> builder)
    {
        builder.ToTable("EmployeeTerritories");

        builder.HasKey(et => et.Id);

        builder.Property(et => et.Id).HasConversion(
            employeeTerritoryId => employeeTerritoryId.Value,
            value => EmployeeTerritoryId.CreateUnique())
            .HasColumnName(nameof(EmployeeTerritoryId))
            .ValueGeneratedOnAdd();

        builder.Property(et => et.EmployeeId)
            .IsRequired();

        builder.Property(et => et.TerritoryId)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne(et => et.Territory)
            .WithOne(ter => ter.EmployeeTerritory)
            .HasForeignKey<EmployeeTerritory>(et => et.TerritoryId);
    }
}
