using MobileStore.Domain.Entities;

namespace MobileStore.Infrastructure.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        // ============================================
        // Basic CRUD
        // ============================================
        Task<Review?> GetByIdAsync(int id);
        Task<IEnumerable<Review>> GetAllAsync();
        Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId);
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(int userId);
        Task<Review> CreateAsync(Review review);
        Task<Review> UpdateAsync(Review review);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);

        // ============================================
        // Rating Calculations
        // ============================================
        Task<double> GetAverageRatingByProductIdAsync(int productId);
        Task<int> GetReviewCountByProductIdAsync(int productId);

        // ============================================
        // Validation
        // ============================================
        Task<bool> UserHasReviewedProductAsync(int userId, int productId);
        Task<Review?> GetUserReviewForProductAsync(int userId, int productId);
    }
}