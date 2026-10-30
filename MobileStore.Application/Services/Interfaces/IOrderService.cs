using MobileStore.Application.DTOs.Order;

namespace MobileStore.Application.Services.Interfaces
{
    public interface IOrderService
    {
        // Basic CRUD
        Task<OrderResponseDto> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync();
        Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto createDto);
        Task<OrderResponseDto> UpdateOrderAsync(int id, UpdateOrderDto updateDto);
        Task<bool> DeleteOrderAsync(int id);

        // User-specific
        Task<IEnumerable<OrderResponseDto>> GetOrdersByUserIdAsync(int userId);
        Task<int> GetOrderCountByUserIdAsync(int userId);

        // Status management
        Task<OrderResponseDto> UpdateOrderStatusAsync(int id, string newStatus);

        // Validation helpers
        Task<bool> OrderExistsAsync(int id);
    }
}