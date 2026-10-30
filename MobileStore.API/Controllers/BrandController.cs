using Microsoft.AspNetCore.Mvc;
using MobileStore.Application.DTOs.Brand;
using MobileStore.Application.Services.Interfaces;

namespace MobileStore.API.Controllers
{
    /// <summary>
    /// Controller for managing brands
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController(IBrandService brandService) : ControllerBase
    {
        private readonly IBrandService _brandService = brandService;

        /// <summary>
        /// Get all brands
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BrandResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BrandResponseDto>>> GetAll()
        {
            var brands = await _brandService.GetAllBrandsAsync();
            return Ok(brands);
        }

        /// <summary>
        /// Get only active brands
        /// </summary>
        [HttpGet("active")]
        [ProducesResponseType(typeof(IEnumerable<BrandResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BrandResponseDto>>> GetActive()
        {
            var brands = await _brandService.GetActiveBrandsAsync();
            return Ok(brands);
        }

        /// <summary>
        /// Get a brand by its ID
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(BrandResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BrandResponseDto>> GetById(int id)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            return Ok(brand);
        }

        /// <summary>
        /// Create a new brand
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(BrandResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BrandResponseDto>> Create([FromBody] CreateBrandDto createDto)
        {
            var brand = await _brandService.CreateBrandAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = brand.Id }, brand);
        }

        /// <summary>
        /// Update an existing brand
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(BrandResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BrandResponseDto>> Update(int id, [FromBody] UpdateBrandDto updateDto)
        {
            var brand = await _brandService.UpdateBrandAsync(id, updateDto);
            return Ok(brand);
        }

        /// <summary>
        /// Delete (soft delete) a brand
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _brandService.DeleteBrandAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Check if a brand exists
        /// </summary>
        [HttpGet("{id:int}/exists")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> Exists(int id)
        {
            var exists = await _brandService.BrandExistsAsync(id);
            return Ok(exists);
        }
    }
}