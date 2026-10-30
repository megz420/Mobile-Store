using Microsoft.AspNetCore.Mvc;
using MobileStore.Application.DTOs.Product;
using MobileStore.Application.Services.Interfaces;

namespace MobileStore.API.Controllers
{
    /// <summary>
    /// Controller for managing products
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        // ============================================
        // GET: api/Products
        // Get all products
        // ============================================
        /// <summary>
        /// Get all products
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductListDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductListDto>>> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        // ============================================
        // GET: api/Products/active
        // Get only active products
        // ============================================
        /// <summary>
        /// Get only active products
        /// </summary>
        [HttpGet("active")]
        [ProducesResponseType(typeof(IEnumerable<ProductListDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductListDto>>> GetActive()
        {
            var products = await _productService.GetActiveProductsAsync();
            return Ok(products);
        }

        // ============================================
        // GET: api/Products/{id}
        // Get product by ID with full details
        // ============================================
        /// <summary>
        /// Get a product by its ID with full details (brand, category, images, reviews)
        /// </summary>
        /// <param name="id">The product ID</param>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductResponseDto>> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }

        // ============================================
        // GET: api/Products/brand/{brandId}
        // Get products by brand
        // ============================================
        /// <summary>
        /// Get all products for a specific brand
        /// </summary>
        /// <param name="brandId">The brand ID</param>
        [HttpGet("brand/{brandId:int}")]
        [ProducesResponseType(typeof(IEnumerable<ProductListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProductListDto>>> GetByBrand(int brandId)
        {
            var products = await _productService.GetProductsByBrandAsync(brandId);
            return Ok(products);
        }

        // ============================================
        // GET: api/Products/category/{categoryId}
        // Get products by category
        // ============================================
        /// <summary>
        /// Get all products for a specific category
        /// </summary>
        /// <param name="categoryId">The category ID</param>
        [HttpGet("category/{categoryId:int}")]
        [ProducesResponseType(typeof(IEnumerable<ProductListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProductListDto>>> GetByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }

        // ============================================
        // GET: api/Products/search?term=xxx
        // Search products
        // ============================================
        /// <summary>
        /// Search products by name, description, brand, or category
        /// </summary>
        /// <param name="term">Search term</param>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<ProductListDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductListDto>>> Search([FromQuery] string term)
        {
            var products = await _productService.SearchProductsAsync(term);
            return Ok(products);
        }

        // ============================================
        // GET: api/Products/price-range?min=10&max=100
        // Get products by price range
        // ============================================
        /// <summary>
        /// Get products within a price range
        /// </summary>
        /// <param name="min">Minimum price</param>
        /// <param name="max">Maximum price</param>
        [HttpGet("price-range")]
        [ProducesResponseType(typeof(IEnumerable<ProductListDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductListDto>>> GetByPriceRange(
            [FromQuery] decimal min,
            [FromQuery] decimal max)
        {
            var products = await _productService.GetProductsByPriceRangeAsync(min, max);
            return Ok(products);
        }

        // ============================================
        // GET: api/Products/low-stock?threshold=10
        // Get low stock products
        // ============================================
        /// <summary>
        /// Get products with low stock (below threshold)
        /// </summary>
        /// <param name="threshold">Stock threshold (default: 10)</param>
        [HttpGet("low-stock")]
        [ProducesResponseType(typeof(IEnumerable<ProductListDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductListDto>>> GetLowStock(
            [FromQuery] int threshold = 10)
        {
            var products = await _productService.GetLowStockProductsAsync(threshold);
            return Ok(products);
        }

        // ============================================
        // GET: api/Products/out-of-stock
        // Get out of stock products
        // ============================================
        /// <summary>
        /// Get products that are out of stock
        /// </summary>
        [HttpGet("out-of-stock")]
        [ProducesResponseType(typeof(IEnumerable<ProductListDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductListDto>>> GetOutOfStock()
        {
            var products = await _productService.GetOutOfStockProductsAsync();
            return Ok(products);
        }

        // ============================================
        // POST: api/Products
        // Create a new product
        // ============================================
        /// <summary>
        /// Create a new product
        /// </summary>
        /// <param name="createDto">Product creation data</param>
        [HttpPost]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductResponseDto>> Create([FromBody] CreateProductDto createDto)
        {
            var product = await _productService.CreateProductAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        // ============================================
        // PUT: api/Products/{id}
        // Update an existing product
        // ============================================
        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <param name="id">The product ID to update</param>
        /// <param name="updateDto">Updated product data</param>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductResponseDto>> Update(
            int id,
            [FromBody] UpdateProductDto updateDto)
        {
            var product = await _productService.UpdateProductAsync(id, updateDto);
            return Ok(product);
        }

        // ============================================
        // DELETE: api/Products/{id}
        // Delete (soft delete) a product
        // ============================================
        /// <summary>
        /// Delete (soft delete) a product
        /// </summary>
        /// <param name="id">The product ID to delete</param>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }

        // ============================================
        // GET: api/Products/{id}/exists
        // Check if product exists
        // ============================================
        /// <summary>
        /// Check if a product exists
        /// </summary>
        /// <param name="id">The product ID</param>
        [HttpGet("{id:int}/exists")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> Exists(int id)
        {
            var exists = await _productService.ProductExistsAsync(id);
            return Ok(exists);
        }
    }
}