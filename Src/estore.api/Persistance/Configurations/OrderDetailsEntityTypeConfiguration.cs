namespace estore.api.Persistance.Configurations;

using estore.api.Models.Aggregates.Orders.Entities;
using estore.api.Models.Aggregates.Orders.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrderDetailsEntityTypeConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.ToTable("OrderDetails");

        builder.HasKey(ordDtl => ordDtl.Id);

        builder.Property(ordDtl => ordDtl.Id).HasConversion(
            orderDetailId => orderDetailId.Value,
            value => new OrderDetailId(value))
            .HasColumnName(nameof(OrderDetailId));

        builder.Property(od => od.OrderId)
            .IsRequired();

        builder.Property(od => od.ProductId)
            .IsRequired();

        builder.Property(od => od.UnitPrice)
            .HasConversion<decimal>()
            .IsRequired();

        builder.Property(od => od.Quantity)
            .IsRequired();

        builder.Property(od => od.Discount)
            .HasConversion<double>()
            .IsRequired();

        builder.HasIndex(od => od.OrderId);

        builder.HasIndex(od => od.ProductId);
    }
}
