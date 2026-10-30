
using MobileStore.Domain.Enums;

namespace MobileStore.Domain.Entities{    
    public class Review
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int UserId { get; set; }

        public Rating Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public  Product Product { get; set; } = null!;

        public  User User { get; set; } = null!;
    }
}