using AutoMapper;
using MobileStore.Application.DTOs.ProductImage;
using MobileStore.Application.Services.Interfaces;
using MobileStore.Domain.Entities;
using MobileStore.Infrastructure.Repositories.Interfaces;

namespace MobileStore.Application.Services.Implementations
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _imageRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductImageService(
            IProductImageRepository imageRepository,
            IProductRepository productRepository,
            IMapper mapper)
        {
            _imageRepository = imageRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        // ============================================
        // Basic CRUD
        // ============================================

        public async Task<ProductImageResponseDto> GetImageByIdAsync(int id)
        {
            var image = await _imageRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Image with ID {id} not found.");

            return _mapper.Map<ProductImageResponseDto>(image);
        }

        public async Task<IEnumerable<ProductImageResponseDto>> GetAllImagesAsync()
        {
            var images = await _imageRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductImageResponseDto>>(images);
        }

        public async Task<IEnumerable<ProductImageResponseDto>> GetImagesByProductIdAsync(int productId)
        {
            if (!await _productRepository.ExistsAsync(productId))
                throw new KeyNotFoundException($"Product with ID {productId} not found.");

            var images = await _imageRepository.GetImagesByProductIdAsync(productId);
            return _mapper.Map<IEnumerable<ProductImageResponseDto>>(images);
        }

        public async Task<ProductImageResponseDto> CreateImageAsync(CreateProductImageDto createDto)
        {
            // Validate product exists
            if (!await _productRepository.ExistsAsync(createDto.ProductId))
                throw new KeyNotFoundException($"Product with ID {createDto.ProductId} not found.");

            var image = _mapper.Map<ProductImage>(createDto);
            var created = await _imageRepository.CreateAsync(image);

            return _mapper.Map<ProductImageResponseDto>(created);
        }

        public async Task<ProductImageResponseDto> UpdateImageAsync(int id, UpdateProductImageDto updateDto)
        {
            var image = await _imageRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Image with ID {id} not found.");

            _mapper.Map(updateDto, image);
            var updated = await _imageRepository.UpdateAsync(image);

            return _mapper.Map<ProductImageResponseDto>(updated);
        }

        public async Task<bool> DeleteImageAsync(int id)
        {
            if (!await _imageRepository.ExistsAsync(id))
                throw new KeyNotFoundException($"Image with ID {id} not found.");

            return await _imageRepository.DeleteAsync(id);
        }

        public async Task<bool> DeleteAllImagesByProductIdAsync(int productId)
        {
            if (!await _productRepository.ExistsAsync(productId))
                throw new KeyNotFoundException($"Product with ID {productId} not found.");

            return await _imageRepository.DeleteAllImagesByProductIdAsync(productId);
        }

        // ============================================
        // Product Image Management
        // ============================================

        public async Task<ProductImageResponseDto?> GetPrimaryImageByProductIdAsync(int productId)
        {
            if (!await _productRepository.ExistsAsync(productId))
                throw new KeyNotFoundException($"Product with ID {productId} not found.");

            var image = await _imageRepository.GetPrimaryImageByProductIdAsync(productId);
            return image != null ? _mapper.Map<ProductImageResponseDto>(image) : null;
        }

        public async Task<int> GetImageCountByProductIdAsync(int productId)
        {
            if (!await _productRepository.ExistsAsync(productId))
                throw new KeyNotFoundException($"Product with ID {productId} not found.");

            return await _imageRepository.GetImageCountByProductIdAsync(productId);
        }

        // ============================================
        // Validation
        // ============================================

        public async Task<bool> ImageExistsAsync(int id)
        {
            return await _imageRepository.ExistsAsync(id);
        }

        public async Task<bool> ProductHasImagesAsync(int productId)
        {
            if (!await _productRepository.ExistsAsync(productId))
                return false;

            return await _imageRepository.GetImageCountByProductIdAsync(productId) > 0;
        }
    }
}