using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileStore.Domain.Entities;

namespace MobileStore.Infrastructure.Persistence.Configurations

{
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(p => p.Price)
               .HasColumnType("decimal(10,2)");

        builder.Property(p => p.StockQuantity)
               .HasDefaultValue(0);

        builder.Property(p => p.IsActive)
               .HasDefaultValue(true);

        builder.Property(p => p.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

       builder.Property(p => p.UpdatedAt);

        builder.HasMany(p => p.ProductImages)
               .WithOne(i => i.Product)
               .HasForeignKey(i => i.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.OrderItems)
               .WithOne(oi => oi.Product)
               .HasForeignKey(oi => oi.ProductId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(p => p.Reviews)
               .WithOne(r => r.Product)
               .HasForeignKey(r => r.ProductId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
}
