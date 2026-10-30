namespace MobileStore.Application.DTOs.Brand
{
    /// <summary>
    /// DTO for updating an existing brand
    /// </summary>
    public class UpdateBrandDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Logo { get; set; }
        public bool IsActive { get; set; } = true; // Default to true when updating a brand
    }
}