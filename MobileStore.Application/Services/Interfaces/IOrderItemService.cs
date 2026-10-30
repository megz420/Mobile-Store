using MobileStore.Application.DTOs.OrderItem;

namespace MobileStore.Application.Services.Interfaces
{
    public interface IOrderItemService
    {
        // Basic CRUD
        Task<OrderItemResponseDto> GetByIdAsync(int id);
        Task<IEnumerable<OrderItemResponseDto>> GetAllAsync();
        Task<OrderItemResponseDto> CreateAsync(CreateOrderItemDto createDto);
        Task<OrderItemResponseDto> UpdateAsync(int id, OrderItemDto updateDto);
        Task<bool> DeleteAsync(int id);

        // Order-specific
        Task<IEnumerable<OrderItemResponseDto>> GetItemsByOrderIdAsync(int orderId);
        Task<decimal> GetTotalAmountForOrderAsync(int orderId); // sum of price * quantity

        // Validation
        Task<bool> ExistsAsync(int id);
    }
}