using AutoMapper;
using MobileStore.Application.DTOs.Order;
using MobileStore.Application.Services.Interfaces;
using MobileStore.Domain.Entities;
using MobileStore.Domain.Enums;
using MobileStore.Infrastructure.Repositories.Interfaces;

namespace MobileStore.Application.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderResponseDto> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id) 
            ?? throw new KeyNotFoundException($"Order with ID {id} not found.");
            return _mapper.Map<OrderResponseDto>(order);
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderResponseDto>>(orders);
        }

        // ============================================
        // Create Order
        // ============================================
        public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto createDto)
        {
            // Map DTO to entity
            var order = _mapper.Map<Order>(createDto);

            // Calculate total amount from items
            decimal total = 0;
            foreach (var item in order.OrderItems)
            {
                total += item.Quantity * item.PriceAtPurchase;
            }
            order.TotalAmount = total;

            // Set default status
            order.OrderStatus = OrderStatus.pending;
            order.CreatedAt = DateTime.UtcNow;

            // Save
            var createdOrder = await _orderRepository.CreateAsync(order);

            return _mapper.Map<OrderResponseDto>(createdOrder);
        }

        public async Task<OrderResponseDto> UpdateOrderAsync(int id, UpdateOrderDto updateDto)
        {
            var order = await _orderRepository.GetByIdAsync(id) 
            ?? throw new KeyNotFoundException($"Order with ID {id} not found.");

            // Map only allowed fields
            _mapper.Map(updateDto, order);

            // Recalculate total if items changed (but we don't update items here)
            // For simplicity, we assume total is recalculated or not changed via update.
            // You might want to add logic to recalculate if items are updated.

            var updatedOrder = await _orderRepository.UpdateAsync(order);
            return _mapper.Map<OrderResponseDto>(updatedOrder);
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            if (!await _orderRepository.ExistsAsync(id))
                throw new KeyNotFoundException($"Order with ID {id} not found.");

            return await _orderRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<OrderResponseDto>> GetOrdersByUserIdAsync(int userId)
        {
            // ensure also id user had this id or not
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<OrderResponseDto>>(orders);
        }

        public async Task<int> GetOrderCountByUserIdAsync(int userId)
        {
            return await _orderRepository.GetOrderCountByUserIdAsync(userId);
        }

        public async Task<OrderResponseDto> UpdateOrderStatusAsync(int id, string newStatus)
        {
            if (!Enum.TryParse<OrderStatus>(newStatus, true, out var status))
                throw new ArgumentException($"Invalid order status: {newStatus}");

            var order = await _orderRepository.GetByIdAsync(id) 
            ?? throw new KeyNotFoundException($"Order with ID {id} not found.");

            order.OrderStatus = status;
            var updatedOrder = await _orderRepository.UpdateAsync(order);

            return _mapper.Map<OrderResponseDto>(updatedOrder);
        }

        public async Task<bool> OrderExistsAsync(int id)
        {
            return await _orderRepository.ExistsAsync(id);
        }
    }
}