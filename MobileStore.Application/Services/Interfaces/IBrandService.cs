using MobileStore.Application.DTOs.Brand;

namespace MobileStore.Application.Services.Interfaces
{
    public interface IBrandService
    {
        Task<BrandResponseDto> GetBrandByIdAsync(int id);
        Task<IEnumerable<BrandResponseDto>> GetAllBrandsAsync();
        Task<IEnumerable<BrandResponseDto>> GetActiveBrandsAsync();
        Task<BrandResponseDto> CreateBrandAsync(CreateBrandDto brandDto);
        Task<BrandResponseDto> UpdateBrandAsync(int id, UpdateBrandDto brandDto);
        Task<bool> DeleteBrandAsync(int id);
        Task<bool> BrandExistsAsync(int id);
        Task<bool> BrandNameExistsAsync(string name, int? excludeId = null);
    }
}