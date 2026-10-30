using AutoMapper;
using MobileStore.Application.DTOs.Brand;
using MobileStore.Application.Services.Interfaces;
using MobileStore.Infrastructure.Repositories.Interfaces;
using MobileStore.Domain.Entities;

namespace MobileStore.Application.Services.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<BrandResponseDto> GetBrandByIdAsync(int id)
        {
            var brand = await _brandRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Brand with ID {id} not found.");
            var response = _mapper.Map<BrandResponseDto>(brand);
            
            // Get product count
            response.ProductCount = await _brandRepository.GetProductCountAsync(id);
            
            return response;
        }

        public async Task<IEnumerable<BrandResponseDto>> GetAllBrandsAsync()
        {
            var brands = await _brandRepository.GetAllAsync();
            var response = _mapper.Map<IEnumerable<BrandResponseDto>>(brands);

            // Add product count for each brand
            foreach (var brandDto in response)
            {
                brandDto.ProductCount = await _brandRepository.GetProductCountAsync(brandDto.Id);
            }

            return response;
        }
        public async Task<IEnumerable<BrandResponseDto>> GetActiveBrandsAsync()
        {
            var brands = await _brandRepository.GetActiveBrandsAsync();
            var response = _mapper.Map<IEnumerable<BrandResponseDto>>(brands);

            foreach (var brandDto in response)
            {
                brandDto.ProductCount = await _brandRepository.GetProductCountAsync(brandDto.Id);
            }

            return response;
        }

        public async Task<BrandResponseDto> CreateBrandAsync(CreateBrandDto createDto)
        {
            // Validate: Check if brand name already exists
            if (await _brandRepository.ExistsByNameAsync(createDto.Name))
                throw new InvalidOperationException($"Brand '{createDto.Name}' already exists.");

            // Map DTO to Entity
            var brand = _mapper.Map<Brand>(createDto);
            
            // Set default values (repository will set CreatedAt and IsActive)
            // But we can set them here as well
            brand.IsActive = true;
            brand.CreatedAt = DateTime.UtcNow;

            // Create brand
            var createdBrand = await _brandRepository.CreateAsync(brand);

            // Map to response DTO
            var response = _mapper.Map<BrandResponseDto>(createdBrand);
            response.ProductCount = 0; // New brand has no products

            return response;
        }

        public async Task<BrandResponseDto> UpdateBrandAsync(int id, UpdateBrandDto updateDto)
        {
            // Check if brand exists
            var brand = await _brandRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Brand with ID {id} not found.");

            // Map updated values to existing entity
            _mapper.Map(updateDto, brand);

            // Update brand (repository will handle saving)
            var updatedBrand = await _brandRepository.UpdateAsync(brand);

            // Map to response
            var response = _mapper.Map<BrandResponseDto>(updatedBrand);
            response.ProductCount = await _brandRepository.GetProductCountAsync(id);

            return response;
        }


        public async Task<bool> DeleteBrandAsync(int id)
        {
            // Check if brand exists
            if (!await _brandRepository.ExistsAsync(id))
                throw new KeyNotFoundException($"Brand with ID {id} not found.");

            // Check if brand has associated products
            var productCount = await _brandRepository.GetProductCountAsync(id);
            if (productCount > 0)
                throw new InvalidOperationException(
                    $"Cannot delete brand with {productCount} associated products. " +
                    "Please reassign or delete the products first.");

            // Soft delete
            return await _brandRepository.DeleteAsync(id);
        }

        public async Task<bool> BrandExistsAsync(int id)
        {
            return await _brandRepository.ExistsAsync(id);
        }

        public async Task<bool> BrandNameExistsAsync(string name, int? excludeId = null)
        {
            return await _brandRepository.ExistsByNameAsync(name, excludeId);
        }
    }

}