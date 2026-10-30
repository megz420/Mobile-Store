
using MobileStore.Domain.Enums;

namespace MobileStore.Domain.Entities{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public decimal TotalAmount { get; set; } // cause you want two decimal points

        public PaymentMethod PaymentMethod { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ShippingAddress { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<OrderItem> OrderItems { get; set; } = [];

        public User User { get; set; } = null!;
    }
}