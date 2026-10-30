using MobileStore.Domain.Entities;

namespace MobileStore.Infrastructure.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<OrderItem?> GetByIdAsync(int id);
        Task<IEnumerable<OrderItem>> GetAllAsync();
        Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId);
        Task<OrderItem> CreateAsync(OrderItem item);
        Task<OrderItem> UpdateAsync(OrderItem item);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}