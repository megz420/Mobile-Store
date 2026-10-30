using MobileStore.Domain.Entities;

namespace MobileStore.Infrastructure.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> CreateAsync(Order order);
        Task<Order> UpdateAsync(Order order);
        Task<bool> DeleteAsync(int id); 
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId);
        Task<int> GetOrderCountByUserIdAsync(int userId);
        
    }
}
