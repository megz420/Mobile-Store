using MobileStore.Application.DTOs.Product;

namespace MobileStore.Application.Services.Interfaces
{
    public interface IProductService
    {
        // Basic CRUD
        Task<ProductResponseDto> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductListDto>> GetAllProductsAsync();
        Task<IEnumerable<ProductListDto>> GetActiveProductsAsync();
        Task<ProductResponseDto> CreateProductAsync(CreateProductDto createDto);
        Task<ProductResponseDto> UpdateProductAsync(int id, UpdateProductDto updateDto);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> ProductExistsAsync(int id);

        // Filtering & Searching
        Task<IEnumerable<ProductListDto>> GetProductsByBrandAsync(int brandId);
        Task<IEnumerable<ProductListDto>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductListDto>> SearchProductsAsync(string searchTerm);
        Task<IEnumerable<ProductListDto>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);

        // Analytics
        Task<IEnumerable<ProductListDto>> GetOutOfStockProductsAsync();
        Task<IEnumerable<ProductListDto>> GetLowStockProductsAsync(int threshold);
    }
}