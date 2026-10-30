namespace MobileStore.Domain.Entities{
    public class Brand
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;  // initialization for any string to avoid warnings

        public string Logo { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public bool IsActive {get;set;} = true;

        // Navigation property
        public ICollection<Product> Products { get; set; } = new List<Product>(); // no virtual avoiding lazy loading
    }
}