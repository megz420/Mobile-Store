using System.ComponentModel.DataAnnotations;

namespace MobileStore.Application.DTOs.Product
{
    public class UpdateProductDto
    {
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Product name must be between 2 and 255 characters")]
        public string? Name { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }

        [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999,999.99")]
        public decimal? Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
        public int? StockQuantity { get; set; }

        public int? BrandId { get; set; }

        public int? CategoryId { get; set; }

        public bool? IsActive { get; set; }

        public List<string> ImageUrls { get; set; } = [];
    }
}