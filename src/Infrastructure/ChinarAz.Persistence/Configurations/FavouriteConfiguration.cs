using ChinarAz.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChinarAz.Persistence.Configurations;

public class FavouriteConfiguration : IEntityTypeConfiguration<Favourite>
{
    public void Configure(EntityTypeBuilder<Favourite> builder)
    {
        builder.ToTable("Favourite");

        builder.HasKey(f => f.Id);

        // 🔗 Relations
        builder.HasOne(f => f.User)
               .WithMany(u => u.Favourites)
               .HasForeignKey(f => f.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.Product)
               .WithMany(p => p.Favourites)
               .HasForeignKey(f => f.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

