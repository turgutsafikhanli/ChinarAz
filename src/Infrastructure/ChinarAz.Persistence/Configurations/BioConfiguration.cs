using ChinarAz.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChinarAz.Persistence.Configurations;

public class BioConfiguration : IEntityTypeConfiguration<Bio>
{
    public void Configure(EntityTypeBuilder<Bio> builder)
    {
        builder.ToTable("Bios");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Key)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(b => b.Value)
               .HasMaxLength(1000);
    }
}

