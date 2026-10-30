using MobileStore.Domain.Entities;

namespace MobileStore.Infrastructure.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        Task<int> GetProductCountAsync(int categoryId);
        Task<Category> CreateAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByNameAsync(
            string name,
            int? excludeId = null);
        
        Task<int> GetTotalCategoryCountAsync();
        Task<int> GetActiveCategoryCountAsync();
    }
}
