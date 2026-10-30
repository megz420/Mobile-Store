using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileStore.Domain.Entities;

namespace MobileStore.Infrastructure.Persistence.Configurations

{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            // Table name
            builder.ToTable("brands");

            // Primary Key
            builder.HasKey(b => b.Id);

            // Name configuration
            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Logo configuration
            builder.Property(b => b.Logo)
                .IsRequired(false)
                .HasMaxLength(255);

            // CreatedAt configuration
            builder.Property(b => b.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // IsActive configuration
            builder.Property(b => b.IsActive)
                .IsRequired();

            // Indexes
            builder.HasIndex(b => b.Name)
                .IsUnique()
                .HasDatabaseName("idx_brand_name");

            builder.HasIndex(b => b.IsActive)
                .HasDatabaseName("idx_brand_is_active");

            // Relationships
            builder.HasMany(b => b.Products)
                .WithOne(p => p.Brand)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
