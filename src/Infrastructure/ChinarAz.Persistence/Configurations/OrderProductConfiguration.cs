using ChinarAz.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChinarAz.Persistence.Configurations;

public class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
{
    public void Configure(EntityTypeBuilder<OrderProduct> builder)
    {
        builder.ToTable("OrderProduct");

        builder.HasKey(op => op.Id);

        builder.Property(op => op.Quantity)
               .HasDefaultValue(0);

        builder.Property(op => op.UnitPrice)
               .HasColumnType("decimal(18,2)");

        builder.Property(op => op.WeightGram)
               .HasColumnType("decimal(18,2)");

        // 🔗 Relations
        builder.HasOne(op => op.Order)
               .WithMany(o => o.OrderProducts)
               .HasForeignKey(op => op.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(op => op.Product)
               .WithMany(p => p.OrderProducts)
               .HasForeignKey(op => op.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

