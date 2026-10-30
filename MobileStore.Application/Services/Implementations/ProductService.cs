using AutoMapper;
using MobileStore.Application.DTOs.Product;
using MobileStore.Application.Services.Interfaces;
using MobileStore.Domain.Entities;
using MobileStore.Infrastructure.Repositories.Interfaces;

namespace MobileStore.Application.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IProductImageRepository productImageRepository, IReviewRepository reviewRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        // ============================================
        // Basic CRUD
        // ============================================

        public async Task<ProductResponseDto> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Product with ID {id} not found.");

            return _mapper.Map<ProductResponseDto>(product);
        }

        public async Task<IEnumerable<ProductListDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductListDto>>(products);
        }

        public async Task<IEnumerable<ProductListDto>> GetActiveProductsAsync()
        {
            var products = await _productRepository.GetActiveProductsAsync();
            return _mapper.Map<IEnumerable<ProductListDto>>(products);
        }

        public async Task<ProductResponseDto> CreateProductAsync(CreateProductDto createDto)
        {
            // Check if product name already exists
            if (await _productRepository.ExistsByNameAsync(createDto.Name))
                throw new InvalidOperationException($"Product '{createDto.Name}' already exists.");

            var product = _mapper.Map<Product>(createDto);
            var created = await _productRepository.CreateAsync(product);

            // 4. Create ProductImage entities for each URL
            if (createDto.ImageUrls != null && createDto.ImageUrls.Any())
            {
                foreach (var imageUrl in createDto.ImageUrls)
                {
                    var productImage = new ProductImage
                    {
                        ProductId = created.Id,
                        ImageUrl = imageUrl,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _productImageRepository.CreateAsync(productImage);
                }
            }

            // 5. Reload the product with all navigation properties (Brand, Category, Images, Reviews)
            var fullProduct = await _productRepository.GetByIdAsync(created.Id);
            return _mapper.Map<ProductResponseDto>(fullProduct);
        }

        public async Task<ProductResponseDto> UpdateProductAsync(int id, UpdateProductDto updateDto)
        {
            var product = await _productRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Product with ID {id} not found.");

            // Check name uniqueness if name is being updated
            if (!string.IsNullOrEmpty(updateDto.Name) &&
                await _productRepository.ExistsByNameAsync(updateDto.Name, id))
                throw new InvalidOperationException($"Product '{updateDto.Name}' already exists.");

            _mapper.Map(updateDto, product);
            var updated = await _productRepository.UpdateAsync(product);

            // Reload to get updated navigation properties
            var fullProduct = await _productRepository.GetByIdAsync(updated.Id);
            return _mapper.Map<ProductResponseDto>(fullProduct);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            if (!await _productRepository.ExistsAsync(id))
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            return await _productRepository.DeleteAsync(id);
        }

        public async Task<bool> ProductExistsAsync(int id)
        {
            return await _productRepository.ExistsAsync(id);
        }

        // ============================================
        // Filtering & Searching
        // ============================================

        public async Task<IEnumerable<ProductListDto>> GetProductsByBrandAsync(int brandId)
        {
            var products = await _productRepository.GetProductsByBrandAsync(brandId);
            return _mapper.Map<IEnumerable<ProductListDto>>(products);
        }

        public async Task<IEnumerable<ProductListDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
            return _mapper.Map<IEnumerable<ProductListDto>>(products);
        }

        public async Task<IEnumerable<ProductListDto>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllProductsAsync();

            var products = await _productRepository.SearchProductsAsync(searchTerm);
            return _mapper.Map<IEnumerable<ProductListDto>>(products);
        }

        public async Task<IEnumerable<ProductListDto>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            var products = await _productRepository.GetProductsByPriceRangeAsync(minPrice, maxPrice);
            return _mapper.Map<IEnumerable<ProductListDto>>(products);
        }

        public async Task<IEnumerable<ProductListDto>> GetOutOfStockProductsAsync()
        {
            var products = await _productRepository.GetOutOfStockProductsAsync();
            return _mapper.Map<IEnumerable<ProductListDto>>(products);
        }

        public async Task<IEnumerable<ProductListDto>> GetLowStockProductsAsync(int threshold)
        {
            var products = await _productRepository.GetLowStockProductsAsync(threshold);
            return _mapper.Map<IEnumerable<ProductListDto>>(products);
        }
    }
}