namespace estore.api.Persistance.Configurations;

using estore.api.Models.Aggregates.Employee;
using estore.api.Models.Aggregates.Employee.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(emp => emp.Id);

        builder.Property(emp => emp.Id).HasConversion(
            employeeId => employeeId.Value,
            value => new EmployeeId(value))
            .HasColumnName(nameof(EmployeeId));

        builder.Property(emp => emp.Title)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(emp => emp.FirstName)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(emp => emp.LastName)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(emp => emp.TitleOfCourtesy)
            .HasMaxLength(25)
            .IsRequired();

        builder.Property(emp => emp.BirthDate)
            .IsRequired();

        builder.Property(emp => emp.HireDate)
            .IsRequired();

        builder.Property(emp => emp.HomePhone)
            .HasMaxLength(24)
            .IsRequired();

        builder.Property(emp => emp.Extension)
            .HasMaxLength(4)
            .IsRequired();

        builder.Property(emp => emp.Photo)
            .HasColumnType("image")
            .IsRequired();

        builder.Property(emp => emp.Notes)
            .HasColumnType("ntext")
            .IsRequired();

        builder.Property(emp => emp.ReportsTo);

        builder.Property(emp => emp.PhotoPath)
            .HasMaxLength(255)
            .IsRequired();

        builder.ComplexProperty(employee => employee.EmployeeAddress,
            navigationBuilder =>
            {
                navigationBuilder
                    .Property(employeeAddress => employeeAddress.Address)
                    .HasMaxLength(60)
                    .HasColumnName("Address");

                navigationBuilder
                    .Property(employeeAddress => employeeAddress.City)
                    .HasMaxLength(15)
                    .HasColumnName("City");

                navigationBuilder
                    .Property(employeeAddress => employeeAddress.Region)
                    .HasMaxLength(15)
                    .HasColumnName("Region");

                navigationBuilder
                    .Property(employeeAddress => employeeAddress.PostalCode)
                    .HasMaxLength(10)
                    .HasColumnName("PostalCode");

                navigationBuilder
                    .Property(employeeAddress => employeeAddress.Country)
                    .HasMaxLength(15)
                    .HasColumnName("Country");
            });

        builder.HasMany(emp => emp.EmployeeTerritories)
            .WithOne(et => et.Employee)
            .HasForeignKey(et => et.EmployeeId)
            .HasPrincipalKey(emp => emp.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasIndex(emp => emp.LastName);
    }
}
