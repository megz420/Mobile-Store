using AutoMapper;
using MobileStore.Application.DTOs.Category;
using MobileStore.Application.Services.Interfaces;
using MobileStore.Infrastructure.Repositories.Interfaces;
using MobileStore.Domain.Entities;

namespace MobileStore.Application.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var response = _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
            
            foreach (var categoryDto in response)
            {
                categoryDto.ProductCount = await _categoryRepository.GetProductCountAsync(categoryDto.Id);
            }
            return response;
        }
        public async Task<IEnumerable<CategoryResponseDto>> GetActiveCategoriesAsync()
        {
            var categories = await _categoryRepository.GetActiveCategoriesAsync();
            var response = _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);

            foreach (var categoryDto in response)
            {
                categoryDto.ProductCount = await _categoryRepository.GetProductCountAsync(categoryDto.Id);
            }

            return response;
        }
        public async Task<CategoryResponseDto> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id) 
            ?? throw new KeyNotFoundException($"Category with ID {id} not found.");

            var response = _mapper.Map<CategoryResponseDto>(category);

            response.ProductCount = await _categoryRepository.GetProductCountAsync(category.Id);
            return response;
        }
        public async Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            if(await _categoryRepository.ExistsByNameAsync(createCategoryDto.Name))
            {
                throw new InvalidOperationException($"Category with name '{createCategoryDto.Name}' already exists.");
            }

            var category = _mapper.Map<Category>(createCategoryDto);
            category.CreatedAt = DateTime.UtcNow;
            var createdCategory = await _categoryRepository.CreateAsync(category);
            var response = _mapper.Map<CategoryResponseDto>(createdCategory);
            response.ProductCount = 0; // New category will have no products initially
            return response;
        }

        public async Task<CategoryResponseDto> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Category with ID {id} not found.");

            _mapper.Map(updateCategoryDto, existingCategory);
            var updatedCategory = await _categoryRepository.UpdateAsync(existingCategory);
            var response = _mapper.Map<CategoryResponseDto>(updatedCategory);
            response.ProductCount = await _categoryRepository.GetProductCountAsync(response.Id);
            return response;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id)??
                throw new KeyNotFoundException($"Category with ID {id} not found.");

            if(await _categoryRepository.GetProductCountAsync(id) > 0)
            {
                throw new InvalidOperationException($"Cannot delete category with ID {id} because it has associated products.\n if you want to delete this category, please first remove or reassign its associated products.");
            }
            return await _categoryRepository.DeleteAsync(id);
        }
        public async Task<bool> CategoryExistsAsync(int id)
        {
            return await _categoryRepository.ExistsAsync(id);
        }
        public async Task<bool> CategoryNameExistsAsync(string name, int? excludeId = null)
        {
            return await _categoryRepository.ExistsByNameAsync(name, excludeId);
        }
        public async Task<int> GetTotalCategoryCountAsync()
        {
            return await _categoryRepository.GetTotalCategoryCountAsync();
        }
        public async Task<int> GetActiveCategoryCountAsync()
        {
            return await _categoryRepository.GetActiveCategoryCountAsync();
        }
    }
}