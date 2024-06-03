namespace estore.api.Persistance.Configurations;

using estore.api.Models.Aggregates.Customer;
using estore.api.Models.Aggregates.Customer.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).HasConversion(
            customerId => customerId.Value,
            value => CustomerId.CreateUnique())
            .HasMaxLength(5)
            .HasColumnName(nameof(CustomerId));

        builder.Property(c => c.CompanyName)
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(c => c.ContactName)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(c => c.ContactTitle)
            .HasMaxLength(30)
            .IsRequired();

        builder.OwnsOne(customer => customer.CustomerAddress,
            navigationBuilder =>
            {
                navigationBuilder
                    .Property(sa => sa.Address)
                    .HasMaxLength(60)
                    .HasColumnName("Address");

                navigationBuilder
                    .Property(sa => sa.City)
                    .HasMaxLength(15)
                    .HasColumnName("City");

                navigationBuilder
                    .Property(sa => sa.Region)
                    .HasMaxLength(15)
                    .HasColumnName("Region");

                navigationBuilder
                    .Property(sa => sa.PostalCode)
                    .HasMaxLength(10)
                    .HasColumnName("PostalCode");

                navigationBuilder
                    .Property(sa => sa.Country)
                    .HasMaxLength(15)
                    .HasColumnName("Country");
            });

        builder.Property(c => c.Phone)
            .HasMaxLength(24)
            .IsRequired();

        builder.Property(c => c.Fax)
            .HasMaxLength(24);

        builder.HasMany(c => c.Orders)
            .WithOne(order => order.Customer)
            .HasForeignKey(order => order.CustomerId)
            .HasPrincipalKey(c => c.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => c.CompanyName);
    }
}
