using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileStore.Domain.Entities;

namespace MobileStore.Infrastructure.Persistence.Configurations

{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            // Table name
            builder.ToTable("product_images");

            // Primary Key
            builder.HasKey(pi => pi.Id);

            // ProductId configuration
            builder.Property(pi => pi.ProductId)
                .IsRequired();

            // ImageUrl configuration
            builder.Property(pi => pi.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            // CreatedAt configuration
            builder.Property(pi => pi.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // Indexes
            builder.HasIndex(pi => pi.ProductId)
                .HasDatabaseName("idx_product_image_product_id");

            // Composite index for product images ordering
            builder.HasIndex(pi => new { pi.ProductId, pi.CreatedAt })
                .HasDatabaseName("idx_product_image_product_created");
        }
    }
}
