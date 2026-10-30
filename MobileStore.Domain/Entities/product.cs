
namespace MobileStore.Domain.Entities{   
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; } = 0;

        public int BrandId { get; set; }

        public int CategoryId { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Brand Brand { get; set; } = null!;
        public Category Category { get; set; } = null!;
        public ICollection<ProductImage> ProductImages { get; set; } = [];
        public ICollection<OrderItem> OrderItems { get; set; } = [];
        public ICollection<Review> Reviews { get; set; } = [];
    }
}