
namespace MobileStore.Application.DTOs.Brand
{
    /// <summary>
    /// DTO for creating a new brand
    /// </summary>
    public class CreateBrandDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Logo { get; set; }
        public bool IsActive { get; set; } = true; // Default to true when creating a new brand
    }
}