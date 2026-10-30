using System.ComponentModel.DataAnnotations;

namespace MobileStore.Application.DTOs.OrderItem
{
    public class CreateOrderItemDto
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal PriceAtPurchase { get; set; }
    }
}