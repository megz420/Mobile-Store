using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileStore.Domain.Entities;
using MobileStore.Domain.Enums;

namespace MobileStore.Infrastructure.Persistence.Configurations

{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Table name
            builder.ToTable("orders");

            // Primary Key
            builder.HasKey(o => o.Id);

            // UserId configuration
            builder.Property(o => o.UserId)
                .IsRequired();

            // TotalAmount configuration
            builder.Property(o => o.TotalAmount)
                .IsRequired()
                .HasPrecision(10, 2);

            // PaymentMethod configuration (Enum as string)
            builder.Property(o => o.PaymentMethod)
                .IsRequired()
                .HasConversion<string>() // Store enum as string in database
                .HasMaxLength(50);

            // OrderStatus configuration (Enum as string)
            builder.Property(o => o.OrderStatus)
                .IsRequired()
                .HasConversion<string>() // Store enum as string in database
                .HasMaxLength(50)
                .HasDefaultValue(OrderStatus.pending);

            // ShippingAddress configuration
            builder.Property(o => o.ShippingAddress)
                .IsRequired()
                .HasMaxLength(500);

            // CreatedAt configuration
            builder.Property(o => o.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // Indexes
            builder.HasIndex(o => o.UserId)
                .HasDatabaseName("idx_order_user_id");

            builder.HasIndex(o => o.OrderStatus)
                .HasDatabaseName("idx_order_status");

            builder.HasIndex(o => o.CreatedAt)
                .HasDatabaseName("idx_order_created_at");

            builder.HasIndex(o => new { o.UserId, o.OrderStatus })
                .HasDatabaseName("idx_order_user_status");

            // Relationships

            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
