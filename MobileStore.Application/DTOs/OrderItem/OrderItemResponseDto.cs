namespace MobileStore.Application.DTOs.OrderItem{    
    public class OrderItemResponseDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Subtotal => Quantity * PriceAtPurchase;
    }
}