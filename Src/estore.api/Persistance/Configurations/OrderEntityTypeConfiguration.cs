namespace estore.api.Persistance.Configurations;

using estore.api.Models.Aggregates.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Aggregates.Orders.ValueObjects;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id).HasConversion(
            orderId => orderId.Value,
            value => new OrderId(value))
            .HasColumnName(nameof(OrderId));

        builder.Property(o => o.CustomerId)
            .HasMaxLength(5)
            .IsRequired();

        builder.Property(o => o.EmployeeId)
            .IsRequired();

        builder.Property(o => o.OrderDate)
            .HasDefaultValueSql("getdate()")
            .IsRequired();

        builder.Property(o => o.RequiredDate)
            .IsRequired();

        builder.Property(o => o.ShippedDate);

        builder.Property(o => o.ShipVia);

        builder.Property(o => o.Freight)
            .HasConversion<decimal>();

        builder.Property(o => o.ShipName)
            .HasMaxLength(40);

        builder.ComplexProperty(order => order.ShippingAddress,
            navigationBuilder =>
            {
                navigationBuilder
                    .Property(shippingAddress => shippingAddress.Address)
                    .HasMaxLength(60)
                    .HasColumnName("ShipAddress");

                navigationBuilder
                    .Property(shippingAddress => shippingAddress.City)
                    .HasMaxLength(15)
                    .HasColumnName("ShipCity");

                navigationBuilder
                    .Property(shippingAddress => shippingAddress.Region)
                    .HasMaxLength(15)
                    .HasColumnName("ShipRegion");

                navigationBuilder
                    .Property(shippingAddress => shippingAddress.PostalCode)
                    .HasMaxLength(10)
                    .HasColumnName("ShipPostalCode");

                navigationBuilder
                    .Property(shippingAddress => shippingAddress.Country)
                    .HasMaxLength(15)
                    .HasColumnName("ShipCountry");
            });

        builder.HasMany(o => o.OrderDetails)
            .WithOne(o => o.Orders)
            .HasForeignKey(orderDetail => orderDetail.OrderId)
            .HasPrincipalKey(o => o.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(o => o.Employee)
            .WithMany(emp => emp.Orders)
            .HasForeignKey(o => o.EmployeeId);

        builder.HasIndex(o => o.OrderDate);

        builder.HasIndex(o => o.ShippedDate);

        builder.HasIndex(o => o.CustomerId);

        builder.HasIndex(o => o.EmployeeId);
    }
}
