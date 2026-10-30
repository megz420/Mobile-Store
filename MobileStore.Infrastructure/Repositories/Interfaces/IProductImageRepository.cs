using MobileStore.Domain.Entities;

namespace MobileStore.Infrastructure.Repositories.Interfaces
{
    public interface IProductImageRepository
    {

        Task<ProductImage?> GetByIdAsync(int id);
        Task<IEnumerable<ProductImage>> GetAllAsync();
        Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(int productId);
        Task<ProductImage> CreateAsync(ProductImage image);
        Task<ProductImage> UpdateAsync(ProductImage image);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        // ============================================
        // Product Image Management
        // ============================================
        Task<ProductImage?> GetPrimaryImageByProductIdAsync(int productId);
        Task<bool> DeleteAllImagesByProductIdAsync(int productId);
        Task<int> GetImageCountByProductIdAsync(int productId);
    }
}