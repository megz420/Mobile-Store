
namespace MobileStore.Domain.Entities{
    public class ProductImage
    {
        public int Id { get; set; }

        public int ProductId { get; set; } // foreign key

        public string ImageUrl { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        // navigation of foreign key
        public Product Product { get; set; } = null!;
    }
}