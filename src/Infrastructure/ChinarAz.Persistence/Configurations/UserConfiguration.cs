using ChinarAz.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChinarAz.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Fullname)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(u => u.PhoneNumber)
               .IsRequired()
               .HasMaxLength(20);

        builder.HasIndex(u => u.PhoneNumber).IsUnique();

        builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(150);

        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

        // 🔗 Relations
        builder.HasMany(u => u.Orders)
               .WithOne(o => o.User)
               .HasForeignKey(o => o.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Favourites)
               .WithOne(f => f.User)
               .HasForeignKey(f => f.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

