using ChinarAz.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChinarAz.Persistence.Configurations;

public class BlogConfiguration : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        builder.ToTable("Blogs");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(b => b.Content)
               .IsRequired();

        builder.Property(b => b.Author)
               .IsRequired()
               .HasMaxLength(100);

        // 🔗 Relations
        builder.HasOne(b => b.Category)
               .WithMany(c => c.Blogs)
               .HasForeignKey(b => b.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(b => b.Images)
            .WithOne(i => i.Blog)
            .HasForeignKey(i => i.BlogId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

