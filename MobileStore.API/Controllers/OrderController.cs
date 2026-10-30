using Microsoft.AspNetCore.Mvc;
using MobileStore.Application.DTOs.Order;
using MobileStore.Application.Services.Interfaces;

namespace MobileStore.API.Controllers
{
    /// <summary>
    /// Controller for managing orders
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;

        // ============================================
        // GET: api/Orders
        // Get all orders
        // ============================================
        /// <summary>
        /// Get all orders
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetAll()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        // ============================================
        // GET: api/Orders/{id}
        // Get an order by its ID
        // ============================================
        /// <summary>
        /// Get an order by its ID with full details (user, items, products)
        /// </summary>
        /// <param name="id">The order ID</param>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderResponseDto>> GetById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return Ok(order);
        }

        // ============================================
        // GET: api/Orders/user/{userId}
        // Get all orders for a specific user
        // ============================================
        /// <summary>
        /// Get all orders for a specific user
        /// </summary>
        /// <param name="userId">The user ID</param>
        [HttpGet("user/{userId:int}")]
        [ProducesResponseType(typeof(IEnumerable<OrderResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetByUserId(int userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        // ============================================
        // GET: api/Orders/user/{userId}/count
        // Get order count for a specific user
        // ============================================
        /// <summary>
        /// Get the number of orders placed by a specific user
        /// </summary>
        /// <param name="userId">The user ID</param>
        [HttpGet("user/{userId:int}/count")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetOrderCountByUserId(int userId)
        {
            var count = await _orderService.GetOrderCountByUserIdAsync(userId);
            return Ok(count);
        }

        // ============================================
        // POST: api/Orders
        // Create a new order
        // ============================================
        /// <summary>
        /// Create a new order with items
        /// </summary>
        /// <param name="createDto">Order creation data</param>
        [HttpPost]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderResponseDto>> Create([FromBody] CreateOrderDto createDto)
        {
            var order = await _orderService.CreateOrderAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        // ============================================
        // PUT: api/Orders/{id}
        // Update an existing order (shipping address, payment method, etc.)
        // ============================================
        /// <summary>
        /// Update an existing order (shipping address, payment method, or status)
        /// </summary>
        /// <param name="id">The order ID to update</param>
        /// <param name="updateDto">Updated order data</param>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderResponseDto>> Update(
            int id,
            [FromBody] UpdateOrderDto updateDto)
        {
            var order = await _orderService.UpdateOrderAsync(id, updateDto);
            return Ok(order);
        }

        // ============================================
        // PATCH: api/Orders/{id}/status
        // Update only the order status
        // ============================================
        /// <summary>
        /// Update the status of an order (pending, processing, shipped, delivered, cancelled)
        /// </summary>
        /// <param name="id">The order ID</param>
        /// <param name="status">New status (e.g., "shipped", "delivered", "cancelled")</param>
        [HttpPatch("{id:int}/status")]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderResponseDto>> UpdateStatus(
            int id,
            [FromBody] string status)
        {
            var order = await _orderService.UpdateOrderStatusAsync(id, status);
            return Ok(order);
        }

        // ============================================
        // DELETE: api/Orders/{id}
        // Delete (hard delete) an order
        // ============================================
        /// <summary>
        /// Delete an order (hard delete, use with caution)
        /// </summary>
        /// <param name="id">The order ID to delete</param>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return NoContent();
        }

        // ============================================
        // GET: api/Orders/{id}/exists
        // Check if an order exists
        // ============================================
        /// <summary>
        /// Check if an order exists
        /// </summary>
        /// <param name="id">The order ID</param>
        [HttpGet("{id:int}/exists")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> Exists(int id)
        {
            var exists = await _orderService.OrderExistsAsync(id);
            return Ok(exists);
        }
    }
}