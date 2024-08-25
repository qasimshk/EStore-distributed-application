namespace estore.api.Persistance.Configurations;

using estore.api.Models.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(pro => pro.ProductId);

        builder.Property(pro => pro.ProductId)
            .ValueGeneratedNever();

        builder.Property(pro => pro.ProductName)
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(pro => pro.SupplierId)
            .IsRequired();

        builder.Property(pro => pro.CategoryId)
            .IsRequired();

        builder.Property(pro => pro.QuantityPerUnit)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(pro => pro.UnitPrice)
            .HasColumnType("money")
            .IsRequired();

        builder.Property(pro => pro.UnitsInStock)
            .HasColumnType("smallint")
            .IsRequired();

        builder.Property(pro => pro.UnitsOnOrder)
            .HasColumnType("smallint")
            .IsRequired();

        builder.Property(pro => pro.ReorderLevel)
            .HasColumnType("smallint")
            .IsRequired();

        builder.Property(pro => pro.Discontinued)
            .IsRequired();

        builder.HasIndex(pro => pro.ProductName);

        builder.HasIndex(pro => pro.ProductId);

        builder.HasIndex(pro => pro.SupplierId);
    }
}
