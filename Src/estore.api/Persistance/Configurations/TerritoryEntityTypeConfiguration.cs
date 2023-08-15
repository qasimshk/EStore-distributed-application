namespace estore.api.Persistance.Configurations;

using estore.api.Models.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TerritoryEntityTypeConfiguration : IEntityTypeConfiguration<Territory>
{
    public void Configure(EntityTypeBuilder<Territory> builder)
    {
        builder.ToTable("Territories");

        builder.HasKey(ter => ter.TerritoryId);

        builder.Property(ter => ter.TerritoryId)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(ter => ter.TerritoryDescription)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ter => ter.RegionId)
            .IsRequired();

        builder.HasOne(ter => ter.Region)
            .WithOne(reg => reg.Territory)
            .HasForeignKey<Territory>(ter => ter.RegionId)
            .IsRequired();
    }
}
