namespace estore.api.Persistance.Configurations;

using estore.api.Models.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SupplierEntityTypeConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers");

        builder.HasKey(sup => sup.SupplierId);

        builder.Property(sup => sup.SupplierId)
            .ValueGeneratedNever();

        builder.Property(sup => sup.CompanyName)
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(sup => sup.ContactName)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(sup => sup.ContactTitle)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(sup => sup.Phone)
            .HasMaxLength(24)
            .IsRequired();

        builder.Property(sup => sup.Fax)
            .HasMaxLength(24);

        builder.Property(sup => sup.HomePage)
            .HasColumnType("ntext");

        builder.OwnsOne(sup => sup.SupplierAddress,
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

        builder.HasOne(sup => sup.Product)
            .WithOne(pro => pro.Supplier)
            .HasForeignKey<Product>(pro => pro.SupplierId)
            .IsRequired();
    }
}
