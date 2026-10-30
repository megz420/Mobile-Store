using Microsoft.AspNetCore.Mvc;
using MobileStore.Application.DTOs.Review;
using MobileStore.Application.Services.Interfaces;
using MobileStore.API.Controllers;
using Microsoft.AspNetCore.Authorization;

namespace MobileStore.API.Controllers
{
    /// <summary>
    /// Controller for managing product reviews
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController(IReviewService reviewService) : BaseApiController
    {
        private readonly IReviewService _reviewService = reviewService;


        // ============================================
        // GET: api/Reviews
        // Get all reviews
        // ============================================
        /// <summary>
        /// Get all reviews
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ReviewResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ReviewResponseDto>>> GetAll()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return Ok(reviews);
        }

        // ============================================
        // GET: api/Reviews/{id}
        // Get a review by its ID
        // ============================================
        /// <summary>
        /// Get a review by its ID with user and product details
        /// </summary>
        /// <param name="id">The review ID</param>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ReviewResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewResponseDto>> GetById(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            return Ok(review);
        }

        // ============================================
        // GET: api/Reviews/product/{productId}
        // Get all reviews for a specific product
        // ============================================
        /// <summary>
        /// Get all reviews for a specific product
        /// </summary>
        /// <param name="productId">The product ID</param>
        [HttpGet("product/{productId:int}")]
        [ProducesResponseType(typeof(IEnumerable<ReviewResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ReviewResponseDto>>> GetByProductId(int productId)
        {
            var reviews = await _reviewService.GetReviewsByProductIdAsync(productId);
            return Ok(reviews);
        }

        // ============================================
        // GET: api/Reviews/product/{productId}/summary
        // Get rating summary for a product
        // ============================================
        /// <summary>
        /// Get rating summary (average, total, distribution) for a product
        /// </summary>
        /// <param name="productId">The product ID</param>
        [HttpGet("product/{productId:int}/summary")]
        [ProducesResponseType(typeof(ProductRatingSummaryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductRatingSummaryDto>> GetProductRatingSummary(int productId)
        {
            var summary = await _reviewService.GetProductRatingSummaryAsync(productId);
            return Ok(summary);
        }

        // ============================================
        // GET: api/Reviews/user/{userId}
        // Get all reviews by a specific user
        // ============================================
        /// <summary>
        /// Get all reviews written by a specific user
        /// </summary>
        /// <param name="userId">The user ID</param>
        [HttpGet("user/{userId:int}")]
        [ProducesResponseType(typeof(IEnumerable<ReviewResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ReviewResponseDto>>> GetByUserId(int userId)
        {
            var reviews = await _reviewService.GetReviewsByUserIdAsync(userId);
            return Ok(reviews);
        }

        // ============================================
        // GET: api/Reviews/user/{userId}/product/{productId}
        // Get a user's review for a specific product
        // ============================================
        /// <summary>
        /// Get a specific user's review for a specific product
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <param name="productId">The product ID</param>
        [HttpGet("user/{userId:int}/product/{productId:int}")]
        [ProducesResponseType(typeof(ReviewResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewResponseDto>> GetUserProductReview(int userId, int productId)
        {
            var review = await _reviewService.GetUserReviewForProductAsync(userId, productId);
            if (review == null)
                return NotFound(new { message = $"No review found for user {userId} on product {productId}" });
            return Ok(review);
        }

        // ============================================
        // POST: api/Reviews
        // Create a new review (Current user ID is automatically used)
        // ============================================

        /// <summary>
        /// Create a new review for a product. The current authenticated user's ID is automatically used.
        /// </summary>
        /// <param name="createDto">The review data to create</param>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ReviewResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewResponseDto>> Create([FromBody] CreateReviewDto createDto)
        {
                if (!IsCurrentUserActive())
        return StatusCode(StatusCodes.Status403Forbidden, "Your account is deactivated. You cannot add a review.");
            // Get the current user's ID from the token
            var currentUserId = GetCurrentUserId();
            // Set the user ID in the DTO (override whatever was sent)

            var review = await _reviewService.CreateReviewAsync(createDto, currentUserId);
            return CreatedAtAction(nameof(GetById), new { id = review.Id }, review);
        }

        // ============================================
        // PUT: api/Reviews/{id}
        // Update an existing review
        // ============================================
        /// <summary>
        /// Update an existing review (rating or comment)
        /// </summary>
        /// <param name="id">The review ID to update</param>
        /// <param name="updateDto">Updated review data</param>
        [HttpPut("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(ReviewResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewResponseDto>> Update(
            int id,
            [FromBody] UpdateReviewDto updateDto)
        {
        // Check if the current user is active
        if (!IsCurrentUserActive())
        return StatusCode(StatusCodes.Status403Forbidden, "Your account is deactivated. You cannot update a review.");

            // Get the review to check ownership
            var existingReview = await _reviewService.GetReviewByIdAsync(id);
            if (existingReview == null)
                return NotFound($"Review with ID {id} not found.");

            // Only the owner can update their review
            if (!IsCurrentUserOwner(existingReview.UserId))
                return StatusCode(StatusCodes.Status403Forbidden, "You can only update your own reviews.");

            var review = await _reviewService.UpdateReviewAsync(id, updateDto);
            return Ok(review);
        }

        // ============================================
        // DELETE: api/Reviews/{id}
        // Delete a review
        // ============================================
        /// <summary>
        /// Delete a review
        /// </summary>
        /// <param name="id">The review ID to delete</param>
        // ============================================
        // DELETE: api/Reviews/{id}
        // Delete a review (Owner or Admin can delete)
        // ============================================
        [HttpDelete("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            // Check if the current user is active
            if (!IsCurrentUserActive())
                return StatusCode(StatusCodes.Status403Forbidden, "Your account is deactivated. You cannot delete a review.");

            // Get the review to check ownership
            var existingReview = await _reviewService.GetReviewByIdAsync(id);
            if (existingReview == null)
                return NotFound($"Review with ID {id} not found.");

            // Owner or Admin can delete
            if (!IsCurrentUserOwner(existingReview.UserId) && !IsCurrentUserAdmin())
                return Forbidden("You can only delete your own reviews.");

            await _reviewService.DeleteReviewAsync(id);
            return NoContent();
        }

        // ============================================
        // GET: api/Reviews/user/{userId}/has-reviewed/{productId}
        // Check if a user has reviewed a product
        // ============================================
        /// <summary>
        /// Check if a user has already reviewed a specific product
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <param name="productId">The product ID</param>
        [HttpGet("user/{userId:int}/has-reviewed/{productId:int}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> HasUserReviewedProduct(int userId, int productId)
        {
            var hasReviewed = await _reviewService.UserHasReviewedProductAsync(userId, productId);
            return Ok(hasReviewed);
        }
    }
}