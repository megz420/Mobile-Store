using Microsoft.AspNetCore.Mvc;
using MobileStore.Application.DTOs.Category;
using MobileStore.Application.Services.Interfaces;

namespace MobileStore.API.Controllers
{
    /// <summary>
    /// Controller for managing categories
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryService categoryService) : ControllerBase
    {
        private readonly ICategoryService _categoryService = categoryService;

        /// <summary>
        /// Get all categories
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAll()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        /// <summary>
        /// Get only active categories
        /// </summary>
        [HttpGet("active")]
        [ProducesResponseType(typeof(IEnumerable<CategoryResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetActive()
        {
            var categories = await _categoryService.GetActiveCategoriesAsync();
            return Ok(categories);
        }

        /// <summary>
        /// Get a category by its ID
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponseDto>> GetById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(category);
        }

        /// <summary>
        /// Create a new category
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryResponseDto>> Create([FromBody] CreateCategoryDto createDto)
        {
            var category = await _categoryService.CreateCategoryAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        /// <summary>
        /// Update an existing category
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryResponseDto>> Update(int id, [FromBody] UpdateCategoryDto updateDto)
        {
            var category = await _categoryService.UpdateCategoryAsync(id, updateDto);
            return Ok(category);
        }

        /// <summary>
        /// Delete (soft delete) a category
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }



        /// <summary>
        /// Check if a category exists
        /// </summary>
        [HttpGet("{id:int}/exists")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> Exists(int id)
        {
            var exists = await _categoryService.CategoryExistsAsync(id);
            return Ok(exists);
        }


        /// <summary>
        /// Check if a category exists by name, optionally excluding a specific ID
        /// </summary>
        [HttpGet("exists")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> NameExists(string name, [FromQuery] int? excludeId = null)
        {
            var exists = await _categoryService.CategoryNameExistsAsync(name, excludeId);
            return Ok(exists);
        }

        /// <summary>
        /// Get total category count
        /// </summary>
        [HttpGet("count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetCount()
        {
            var count = await _categoryService.GetTotalCategoryCountAsync();
            return Ok(count);
        }

        /// <summary>
        /// Get active category count
        /// </summary>
        [HttpGet("count/active")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetActiveCount()
        {
            var count = await _categoryService.GetActiveCategoryCountAsync();
            return Ok(count);
        }
    }
}