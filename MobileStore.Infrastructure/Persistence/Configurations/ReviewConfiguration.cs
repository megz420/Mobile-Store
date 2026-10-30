using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileStore.Domain.Entities;

namespace MobileStore.Infrastructure.Persistence.Configurations

{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            // Table name
            builder.ToTable("reviews");

            // Primary Key
            builder.HasKey(r => r.Id);

            // ProductId configuration
            builder.Property(r => r.ProductId)
                .IsRequired();

            // UserId configuration
            builder.Property(r => r.UserId)
                .IsRequired();

            // Rating configuration (Enum as string)
            builder.Property(r => r.Rating)
                .IsRequired();

            // Comment configuration
            builder.Property(r => r.Comment)
                .IsRequired(false)
                .HasMaxLength(1000);

            // CreatedAt configuration
            builder.Property(r => r.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // Indexes
            builder.HasIndex(r => r.ProductId)
                .HasDatabaseName("idx_review_product_id");

            builder.HasIndex(r => r.UserId)
                .HasDatabaseName("idx_review_user_id");

            builder.HasIndex(r => r.Rating)
                .HasDatabaseName("idx_review_rating");

            builder.HasIndex(r => r.CreatedAt)
                .HasDatabaseName("idx_review_created_at");

            // Unique constraint: One review per user per product
            builder.HasIndex(r => new { r.UserId, r.ProductId })
                .IsUnique()
                .HasDatabaseName("unique_user_product_review");
        }
    }
}
