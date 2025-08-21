using ChinarAz.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChinarAz.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(p => p.IsWeighted)
               .IsRequired()
               .HasDefaultValue(false);

        // 🔗 Relations
        builder.HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Images)
               .WithOne(i => i.Product)
               .HasForeignKey(i => i.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Favourites)
               .WithOne(f => f.Product)
               .HasForeignKey(f => f.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.OrderProducts)
               .WithOne(op => op.Product)
               .HasForeignKey(op => op.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

