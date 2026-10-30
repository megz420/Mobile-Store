using MobileStore.Application.DTOs.Review;

namespace MobileStore.Application.Services.Interfaces
{
    public interface IReviewService
    {
        // ============================================
        // Basic CRUD
        // ============================================
        Task<ReviewResponseDto> GetReviewByIdAsync(int id);
        Task<IEnumerable<ReviewResponseDto>> GetAllReviewsAsync();
        Task<IEnumerable<ReviewResponseDto>> GetReviewsByProductIdAsync(int productId);
        Task<IEnumerable<ReviewResponseDto>> GetReviewsByUserIdAsync(int userId);
        Task<ReviewResponseDto> CreateReviewAsync(CreateReviewDto createDto , int userId);
        Task<ReviewResponseDto> UpdateReviewAsync(int id, UpdateReviewDto updateDto);
        Task<bool> DeleteReviewAsync(int id);

        // ============================================
        // Rating & Analytics
        // ============================================
        Task<ProductRatingSummaryDto> GetProductRatingSummaryAsync(int productId);
        Task<double> GetAverageRatingByProductIdAsync(int productId);
        Task<int> GetReviewCountByProductIdAsync(int productId);

        // ============================================
        // Validation
        // ============================================
        Task<bool> UserHasReviewedProductAsync(int userId, int productId);
        Task<ReviewResponseDto?> GetUserReviewForProductAsync(int userId, int productId);
    }
}