using AutoMapper;
using MobileStore.Application.DTOs.Review;
using MobileStore.Application.Services.Interfaces;
using MobileStore.Domain.Entities;
using MobileStore.Domain.Enums;
using MobileStore.Infrastructure.Repositories.Interfaces;

namespace MobileStore.Application.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ReviewService(
            IReviewRepository reviewRepository,
            IProductRepository productRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        // ============================================
        // Basic CRUD
        // ============================================

        public async Task<ReviewResponseDto> GetReviewByIdAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Review with ID {id} not found.");

            return _mapper.Map<ReviewResponseDto>(review);
        }

        public async Task<IEnumerable<ReviewResponseDto>> GetAllReviewsAsync()
        {
            var reviews = await _reviewRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ReviewResponseDto>>(reviews);
        }

        public async Task<IEnumerable<ReviewResponseDto>> GetReviewsByProductIdAsync(int productId)
        {
            if (!await _productRepository.ExistsAsync(productId))
                throw new KeyNotFoundException($"Product with ID {productId} not found.");

            var reviews = await _reviewRepository.GetReviewsByProductIdAsync(productId);
            return _mapper.Map<IEnumerable<ReviewResponseDto>>(reviews);
        }

        public async Task<IEnumerable<ReviewResponseDto>> GetReviewsByUserIdAsync(int userId)
        {
            if (!await _userRepository.ExistsAsync(userId))
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            var reviews = await _reviewRepository.GetReviewsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<ReviewResponseDto>>(reviews);
        }

        public async Task<ReviewResponseDto> CreateReviewAsync(CreateReviewDto createDto, int userId)
        {
            // Validate product exists
            if (!await _productRepository.ExistsAsync(createDto.ProductId))
                throw new KeyNotFoundException($"Product with ID {createDto.ProductId} not found.");

            // Validate user exists
            if (!await _userRepository.ExistsAsync(userId))
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            // Check if user already reviewed this product
            if (await _reviewRepository.UserHasReviewedProductAsync(userId, createDto.ProductId))
                throw new InvalidOperationException("You have already reviewed this product.");

            var review = _mapper.Map<Review>(createDto);
            review.Rating = (Rating)createDto.Rating;
            review.UserId = userId;

            var created = await _reviewRepository.CreateAsync(review);


            return _mapper.Map<ReviewResponseDto>(created);
        }

        public async Task<ReviewResponseDto> UpdateReviewAsync(int id, UpdateReviewDto updateDto)
        {
            var review = await _reviewRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Review with ID {id} not found.");

            var oldRating = review.Rating;
            var productId = review.ProductId;

            _mapper.Map(updateDto, review);

            if (updateDto.Rating.HasValue)
                review.Rating = (Rating)updateDto.Rating.Value;

            var updated = await _reviewRepository.UpdateAsync(review);


            return _mapper.Map<ReviewResponseDto>(updated);
        }

        public async Task<bool> DeleteReviewAsync(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Review with ID {id} not found.");

            var productId = review.ProductId;
            var deleted = await _reviewRepository.DeleteAsync(id);

            return deleted;
        }

        // ============================================
        // Rating & Analytics
        // ============================================

        public async Task<ProductRatingSummaryDto> GetProductRatingSummaryAsync(int productId)
        {
            if (!await _productRepository.ExistsAsync(productId))
                throw new KeyNotFoundException($"Product with ID {productId} not found.");

            var product = await _productRepository.GetByIdAsync(productId);
            var averageRating = await _reviewRepository.GetAverageRatingByProductIdAsync(productId);
            var totalReviews = await _reviewRepository.GetReviewCountByProductIdAsync(productId);
            return new ProductRatingSummaryDto
            {
                ProductId = productId,
                ProductName = product?.Name ?? string.Empty,
                AverageRating = averageRating,
                TotalReviews = totalReviews
            };
        }

        public async Task<double> GetAverageRatingByProductIdAsync(int productId)
        {
            return await _reviewRepository.GetAverageRatingByProductIdAsync(productId);
        }

        public async Task<int> GetReviewCountByProductIdAsync(int productId)
        {
            return await _reviewRepository.GetReviewCountByProductIdAsync(productId);
        }
        // ============================================
        // Validation
        // ============================================

        public async Task<bool> UserHasReviewedProductAsync(int userId, int productId)
        {
            return await _reviewRepository.UserHasReviewedProductAsync(userId, productId);
        }

        public async Task<ReviewResponseDto?> GetUserReviewForProductAsync(int userId, int productId)
        {
            var review = await _reviewRepository.GetUserReviewForProductAsync(userId, productId);
            return review != null ? _mapper.Map<ReviewResponseDto>(review) : null;
        }
    }
}