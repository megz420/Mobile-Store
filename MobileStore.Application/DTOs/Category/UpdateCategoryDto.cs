namespace MobileStore.Application.DTOs.Category
{
    public class UpdateCategoryDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true; // Default to true when updating a category
    }
}