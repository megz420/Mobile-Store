using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileStore.Domain.Entities;

namespace MobileStore.Infrastructure.Persistence.Configurations

{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // Table name
            builder.ToTable("order_items");

            // Primary Key
            builder.HasKey(oi => oi.Id);

            // OrderId configuration
            builder.Property(oi => oi.OrderId)
                .IsRequired();

            // ProductId configuration
            builder.Property(oi => oi.ProductId)
                .IsRequired();

            // Quantity configuration
            builder.Property(oi => oi.Quantity)
                .IsRequired();

            // PriceAtPurchase configuration
            builder.Property(oi => oi.PriceAtPurchase)
                .IsRequired()
                .HasPrecision(10, 2);

            // CreatedAt configuration
            builder.Property(oi => oi.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // Indexes
            builder.HasIndex(oi => oi.OrderId)
                .HasDatabaseName("idx_order_item_order_id");

            builder.HasIndex(oi => oi.ProductId)
                .HasDatabaseName("idx_order_item_product_id");

            // Composite index for better query performance
            builder.HasIndex(oi => new { oi.OrderId, oi.ProductId })
                .HasDatabaseName("idx_order_item_order_product");
        }
    }
}
