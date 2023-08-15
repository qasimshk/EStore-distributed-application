namespace estore.api.Persistance.Configurations;

using estore.api.Models.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.CategoryId);

        builder.Property(c => c.CategoryName)
            .HasMaxLength(15)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasColumnType("text");

        builder.Property(c => c.Picture)
            .HasColumnType("image")
            .IsRequired();

        builder.HasIndex(c => c.CategoryName);

        builder.HasMany(cat => cat.Products)
            .WithOne(pro => pro.Category)
            .HasForeignKey(pro => pro.CategoryId)
            .HasPrincipalKey(cat => cat.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
