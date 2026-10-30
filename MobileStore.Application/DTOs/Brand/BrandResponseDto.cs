namespace MobileStore.Application.DTOs.Brand
{
    /// <summary>
    /// DTO for brand response
    /// </summary>
    public class BrandResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Logo { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ProductCount { get; set; } // Number of products associated with the brand
    }
}