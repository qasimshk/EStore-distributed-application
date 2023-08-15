namespace estore.api.Persistance.Configurations;

using estore.api.Models.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RegionEntityTypeConfiguration : IEntityTypeConfiguration<Region>
{
    public void Configure(EntityTypeBuilder<Region> builder)
    {
        builder.ToTable(nameof(Region));

        builder.HasKey(reg => reg.RegionId);

        builder.Property(reg => reg.RegionDescription)
            .HasMaxLength(50)
            .IsRequired();
    }
}
