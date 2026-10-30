using Microsoft.AspNetCore.Mvc;
using MobileStore.Application.DTOs.ProductImage;
using MobileStore.Application.Services.Interfaces;

namespace MobileStore.API.Controllers
{
    /// <summary>
    /// Controller for managing product images
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImagesController(IProductImageService productImageService) : ControllerBase
    {
        private readonly IProductImageService _productImageService = productImageService;

        // ============================================
        // GET: api/ProductImages
        // Get all product images
        // ============================================
        /// <summary>
        /// Get all product images
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductImageResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductImageResponseDto>>> GetAll()
        {
            var images = await _productImageService.GetAllImagesAsync();
            return Ok(images);
        }

        // ============================================
        // GET: api/ProductImages/{id}
        // Get an image by its ID
        // ============================================
        /// <summary>
        /// Get a product image by its ID
        /// </summary>
        /// <param name="id">The image ID</param>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductImageResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductImageResponseDto>> GetById(int id)
        {
            var image = await _productImageService.GetImageByIdAsync(id);
            return Ok(image);
        }

        // ============================================
        // GET: api/ProductImages/product/{productId}
        // Get all images for a specific product
        // ============================================
        /// <summary>
        /// Get all images for a specific product
        /// </summary>
        /// <param name="productId">The product ID</param>
        [HttpGet("product/{productId:int}")]
        [ProducesResponseType(typeof(IEnumerable<ProductImageResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProductImageResponseDto>>> GetByProductId(int productId)
        {
            var images = await _productImageService.GetImagesByProductIdAsync(productId);
            return Ok(images);
        }

        // ============================================
        // GET: api/ProductImages/product/{productId}/primary
        // Get the primary image for a product
        // ============================================
        /// <summary>
        /// Get the primary (first) image for a product
        /// </summary>
        /// <param name="productId">The product ID</param>
        [HttpGet("product/{productId:int}/primary")]
        [ProducesResponseType(typeof(ProductImageResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductImageResponseDto>> GetPrimaryByProductId(int productId)
        {
            var image = await _productImageService.GetPrimaryImageByProductIdAsync(productId);
            if (image == null)
                return NotFound(new { message = $"No primary image found for product ID {productId}" });
            return Ok(image);
        }

        // ============================================
        // GET: api/ProductImages/product/{productId}/count
        // Get image count for a product
        // ============================================
        /// <summary>
        /// Get the number of images for a product
        /// </summary>
        /// <param name="productId">The product ID</param>
        [HttpGet("product/{productId:int}/count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetImageCountByProductId(int productId)
        {
            var count = await _productImageService.GetImageCountByProductIdAsync(productId);
            return Ok(count);
        }

        // ============================================
        // POST: api/ProductImages
        // Add a new image to a product
        // ============================================
        /// <summary>
        /// Add a new image to a product
        /// </summary>
        /// <param name="createDto">Image creation data</param>
        [HttpPost]
        [ProducesResponseType(typeof(ProductImageResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductImageResponseDto>> Create([FromBody] CreateProductImageDto createDto)
        {
            var image = await _productImageService.CreateImageAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = image.Id }, image);
        }

        // ============================================
        // PUT: api/ProductImages/{id}
        // Update an image URL
        // ============================================
        /// <summary>
        /// Update an existing product image
        /// </summary>
        /// <param name="id">The image ID to update</param>
        /// <param name="updateDto">Updated image data</param>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ProductImageResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductImageResponseDto>> Update(
            int id,
            [FromBody] UpdateProductImageDto updateDto)
        {
            var image = await _productImageService.UpdateImageAsync(id, updateDto);
            return Ok(image);
        }

        // ============================================
        // DELETE: api/ProductImages/{id}
        // Delete an image
        // ============================================
        /// <summary>
        /// Delete a product image
        /// </summary>
        /// <param name="id">The image ID to delete</param>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _productImageService.DeleteImageAsync(id);
            return NoContent();
        }

        // ============================================
        // DELETE: api/ProductImages/product/{productId}
        // Delete all images for a product
        // ============================================
        /// <summary>
        /// Delete all images for a specific product
        /// </summary>
        /// <param name="productId">The product ID</param>
        [HttpDelete("product/{productId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAllByProductId(int productId)
        {
            await _productImageService.DeleteAllImagesByProductIdAsync(productId);
            return NoContent();
        }

        // ============================================
        // GET: api/ProductImages/{id}/exists
        // Check if an image exists
        // ============================================
        /// <summary>
        /// Check if a product image exists
        /// </summary>
        /// <param name="id">The image ID</param>
        [HttpGet("{id:int}/exists")]  // for routing, use the image ID
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)] // attribute for Swagger documentation
        public async Task<ActionResult<bool>> Exists(int id) // action result returns with http status code and boolean value
        {
            var exists = await _productImageService.ImageExistsAsync(id);
            return Ok(exists);
        }
    }
}