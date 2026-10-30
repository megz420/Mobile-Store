using MobileStore.Application.DTOs.OrderItem;
using MobileStore.Domain.Enums;

namespace MobileStore.Application.DTOs.Order
{
    public class CreateOrderDto
    {
        public int UserId { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string ShippingAddress { get; set; } = string.Empty; 

        public List<OrderItemDto> OrderItems {get; set;} = [];

    }
}