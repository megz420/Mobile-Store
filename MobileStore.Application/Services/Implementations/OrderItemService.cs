using AutoMapper;
using MobileStore.Application.DTOs.OrderItem;
using MobileStore.Application.Services.Interfaces;
using MobileStore.Domain.Entities;
using MobileStore.Infrastructure.Repositories.Interfaces;

namespace MobileStore.Application.Services.Implementations
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository; // For updating order total after item changes
        private readonly IMapper _mapper;

        public OrderItemService(IOrderItemRepository orderItemRepository, IOrderRepository orderRepository, IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderItemResponseDto> GetByIdAsync(int id)
        {
            var item = await _orderItemRepository.GetByIdAsync(id) 
            ?? throw new KeyNotFoundException($"Order item with ID {id} not found.");
            
            return _mapper.Map<OrderItemResponseDto>(item);
        }

        public async Task<IEnumerable<OrderItemResponseDto>> GetAllAsync()
        {
            var items = await _orderItemRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderItemResponseDto>>(items);
        }

        public async Task<OrderItemResponseDto> CreateAsync(CreateOrderItemDto createDto)
        {
            // Check if order exists (we'll assume OrderRepository is not injected here, maybe call service? But we'll rely on FK constraint)
            // We could optionally validate the order ID exists via IOrderService, but we'll keep it simple.
            var item = _mapper.Map<OrderItem>(createDto);
            item.CreatedAt = DateTime.UtcNow;

            var created = await _orderItemRepository.CreateAsync(item);

            // Optionally update the Order's TotalAmount
            await UpdateOrderTotalAsync(created.OrderId);

            return _mapper.Map<OrderItemResponseDto>(created);
        }

        public async Task<OrderItemResponseDto> UpdateAsync(int id, OrderItemDto updateDto)
        {
            var item = await _orderItemRepository.GetByIdAsync(id) 
            ?? throw new KeyNotFoundException($"Order item with ID {id} not found.");

            _mapper.Map(updateDto, item); // only update quantity and price maybe
            var updated = await _orderItemRepository.UpdateAsync(item);

            // Recalculate order total
            await UpdateOrderTotalAsync(updated.OrderId);

            return _mapper.Map<OrderItemResponseDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _orderItemRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Order item with ID {id} not found.");

            var orderId = item.OrderId;
            var deleted = await _orderItemRepository.DeleteAsync(id);

            if (deleted)
                await UpdateOrderTotalAsync(orderId);

            return deleted;
        }

        // ============================================
        // Get all items for a specific order
        // ============================================
        public async Task<IEnumerable<OrderItemResponseDto>> GetItemsByOrderIdAsync(int orderId)
        {
            var items = await _orderItemRepository.GetItemsByOrderIdAsync(orderId);
            return _mapper.Map<IEnumerable<OrderItemResponseDto>>(items);
        }

        // ============================================
        // Get total amount for an order (sum of price*quantity)
        // ============================================
        public async Task<decimal> GetTotalAmountForOrderAsync(int orderId)
        {
            var items = await _orderItemRepository.GetItemsByOrderIdAsync(orderId);
            return items.Sum(i => i.Quantity * i.PriceAtPurchase);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _orderItemRepository.ExistsAsync(id);
        }


        private async Task UpdateOrderTotalAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId) 
            ?? throw new KeyNotFoundException($"Order with ID {orderId} not found.");

            // حساب المجموع الكلي
            decimal total = order.OrderItems.Sum(item => item.Quantity * item.PriceAtPurchase);
            order.TotalAmount = total;


            await _orderRepository.UpdateAsync(order);
        }
    }
}