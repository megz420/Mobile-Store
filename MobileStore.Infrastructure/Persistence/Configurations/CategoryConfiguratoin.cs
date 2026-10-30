using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MobileStore.Domain.Entities;

namespace MobileStore.Infrastructure.Persistence.Configurations

{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Table name
            builder.ToTable("categories");

            // Primary Key
            builder.HasKey(c => c.Id);

            // Name configuration
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Description configuration
            builder.Property(c => c.Description)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnType("text");

            // CreatedAt configuration
            builder.Property(c => c.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // IsActive configuration
            builder.Property(c => c.IsActive)
                .IsRequired();

            // Indexes
            builder.HasIndex(c => c.Name)
                .IsUnique()
                .HasDatabaseName("idx_category_name");

            builder.HasIndex(c => c.IsActive)
                .HasDatabaseName("idx_category_is_active");

            // Relationships
            builder.HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
