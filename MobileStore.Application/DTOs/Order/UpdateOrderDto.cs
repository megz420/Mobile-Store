using System.ComponentModel.DataAnnotations;

namespace MobileStore.Application.DTOs.Order
{
    public class UpdateOrderDto
    {
        [StringLength(500)]
        public string? ShippingAddress { get; set; }

        [StringLength(50)]
        public string? PaymentMethod { get; set; }

        // Optionally update order status
        [StringLength(50)]
        public string? OrderStatus { get; set; }
    }
}