namespace MobileStore.Application.DTOs.ProductImage
{
    public class ProductImageResponseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        
        // Product info
        public string? ProductName { get; set; }
    }
}