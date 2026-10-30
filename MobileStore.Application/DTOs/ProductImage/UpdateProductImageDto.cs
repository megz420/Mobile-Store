using System.ComponentModel.DataAnnotations;

namespace MobileStore.Application.DTOs.ProductImage
{
    public class UpdateProductImageDto
    {
        [StringLength(500, ErrorMessage = "Image URL cannot exceed 500 characters")]
        [Url(ErrorMessage = "Invalid URL format")]
        public string? ImageUrl { get; set; }
    }
}