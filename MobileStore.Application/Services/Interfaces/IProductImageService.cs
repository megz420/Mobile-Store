using MobileStore.Application.DTOs.ProductImage;

namespace MobileStore.Application.Services.Interfaces
{
    public interface IProductImageService
    {
        // ============================================
        // Basic CRUD
        // ============================================
        Task<ProductImageResponseDto> GetImageByIdAsync(int id);
        Task<IEnumerable<ProductImageResponseDto>> GetAllImagesAsync();
        Task<IEnumerable<ProductImageResponseDto>> GetImagesByProductIdAsync(int productId);
        Task<ProductImageResponseDto> CreateImageAsync(CreateProductImageDto createDto);
        Task<ProductImageResponseDto> UpdateImageAsync(int id, UpdateProductImageDto updateDto);
        Task<bool> DeleteImageAsync(int id);
        Task<bool> DeleteAllImagesByProductIdAsync(int productId);

        // ============================================
        // Product Image Management
        // ============================================
        Task<ProductImageResponseDto?> GetPrimaryImageByProductIdAsync(int productId);
        Task<int> GetImageCountByProductIdAsync(int productId);

        // ============================================
        // Validation
        // ============================================
        Task<bool> ImageExistsAsync(int id);
        Task<bool> ProductHasImagesAsync(int productId);
    }
}