using MobileStore.Application.DTOs.OrderItem;


namespace MobileStore.Application.DTOs.Order
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string OrderStatus { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;

        // Navigation properties
        public List<OrderItemResponseDto> OrderItems { get; set; } = [];
        public UserSummaryDto? User { get; set; }
    }

    // For user summary inside order response
    public class UserSummaryDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}
