using ChinarAz.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChinarAz.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasIndex(c => c.Name).IsUnique();

        // 🔗 Relations
        builder.HasMany(c => c.Products)
               .WithOne(p => p.Category)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Blogs)
               .WithOne(b => b.Category)
               .HasForeignKey(b => b.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
