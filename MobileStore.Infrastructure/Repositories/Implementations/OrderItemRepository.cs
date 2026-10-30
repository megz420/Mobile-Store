using MobileStore.Infrastructure.Repositories.Interfaces;
using MobileStore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MobileStore.Domain.Entities;

namespace MobileStore.Infrastructure.Repositories.Implementations
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AppDbContext _context;
        public OrderItemRepository( AppDbContext context )
        {
            _context = context;
        }
        public async Task<OrderItem?> GetByIdAsync(int id)
        {
            return await _context.OrderItems
                .Include(oi => oi.Product)
                .FirstOrDefaultAsync(oi => oi.Id == id);
        }
        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await _context.OrderItems
                .Include(oi => oi.Product)
                .ToListAsync();
        }
        public async Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId)
        {
            return await _context.OrderItems
                .Include(oi => oi.Product)
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }
        public async Task<OrderItem> CreateAsync(OrderItem item)
        {
            if(!await _context.Orders.AnyAsync(o => o.Id == item.OrderId))
            {
                throw new InvalidOperationException($"Order with ID {item.OrderId} does not exist.");
            }

            await _context.OrderItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }
        public async Task<OrderItem> UpdateAsync(OrderItem item)
        {
            if(!await _context.Orders.AnyAsync(o => o.Id == item.OrderId))
            {
                throw new InvalidOperationException($"Order with ID {item.OrderId} does not exist.");
            }
            _context.OrderItems.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.OrderItems
                .Include(oi => oi.Product)
                .FirstOrDefaultAsync(oi => oi.Id == id);
            if (item == null) return false;

            _context.OrderItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.OrderItems.AnyAsync(e => e.Id == id);
        }
        
    }
}