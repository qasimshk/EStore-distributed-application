namespace estore.api.Persistance.Configurations;

using estore.api.Models.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ShipperEntityTypeConfiguration : IEntityTypeConfiguration<Shipper>
{
    public void Configure(EntityTypeBuilder<Shipper> builder)
    {
        builder.ToTable("Shippers");

        builder.HasKey(shp => shp.ShipperId);

        builder.Property(shp => shp.CompanyName)
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(shp => shp.Phone)
            .HasMaxLength(24)
            .IsRequired();
    }
}
