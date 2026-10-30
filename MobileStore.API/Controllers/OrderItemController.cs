using Microsoft.AspNetCore.Mvc;
using MobileStore.Application.DTOs.OrderItem;
using MobileStore.Application.Services.Interfaces;

namespace MobileStore.API.Controllers
{
    /// <summary>
    /// Controller for managing order items
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController(IOrderItemService orderItemService) : ControllerBase
    {
        private readonly IOrderItemService _orderItemService = orderItemService;

        // ============================================
        // GET: api/OrderItems
        // Get all order items
        // ============================================
        /// <summary>
        /// Get all order items
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderItemResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrderItemResponseDto>>> GetAll()
        {
            var items = await _orderItemService.GetAllAsync();
            return Ok(items);
        }

        // ============================================
        // GET: api/OrderItems/{id}
        // Get an order item by its ID
        // ============================================
        /// <summary>
        /// Get an order item by its ID with product details
        /// </summary>
        /// <param name="id">The order item ID</param>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(OrderItemResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderItemResponseDto>> GetById(int id)
        {
            var item = await _orderItemService.GetByIdAsync(id);
            return Ok(item);
        }

        // ============================================
        // GET: api/OrderItems/order/{orderId}
        // Get all items for a specific order
        // ============================================
        /// <summary>
        /// Get all items for a specific order with product details
        /// </summary>
        /// <param name="orderId">The order ID</param>
        [HttpGet("order/{orderId:int}")]
        [ProducesResponseType(typeof(IEnumerable<OrderItemResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OrderItemResponseDto>>> GetByOrderId(int orderId)
        {
            var items = await _orderItemService.GetItemsByOrderIdAsync(orderId);
            return Ok(items);
        }

        // ============================================
        // GET: api/OrderItems/order/{orderId}/total
        // Get total amount for an order
        // ============================================
        /// <summary>
        /// Get the total amount for an order (sum of price * quantity for all items)
        /// </summary>
        /// <param name="orderId">The order ID</param>
        [HttpGet("order/{orderId:int}/total")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        public async Task<ActionResult<decimal>> GetOrderTotal(int orderId)
        {
            var total = await _orderItemService.GetTotalAmountForOrderAsync(orderId);
            return Ok(total);
        }

        // ============================================
        // POST: api/OrderItems
        // Add a new item to an order
        // ============================================
        /// <summary>
        /// Add a new item to an order
        /// </summary>
        /// <param name="createDto">Order item creation data</param>
        [HttpPost]
        [ProducesResponseType(typeof(OrderItemResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderItemResponseDto>> Create([FromBody] CreateOrderItemDto createDto)
        {
            var item = await _orderItemService.CreateAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        // ============================================
        // PUT: api/OrderItems/{id}
        // Update an existing order item (quantity or price)
        // ============================================
        /// <summary>
        /// Update an existing order item (quantity or price at purchase)
        /// </summary>
        /// <param name="id">The order item ID to update</param>
        /// <param name="updateDto">Updated order item data</param>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(OrderItemResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderItemResponseDto>> Update(
            int id,
            [FromBody] OrderItemDto updateDto)
        {
            var item = await _orderItemService.UpdateAsync(id, updateDto);
            return Ok(item);
        }

        // ============================================
        // DELETE: api/OrderItems/{id}
        // Delete an order item
        // ============================================
        /// <summary>
        /// Delete an order item (this will recalculate the order total)
        /// </summary>
        /// <param name="id">The order item ID to delete</param>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderItemService.DeleteAsync(id);
            return NoContent();
        }

        // ============================================
        // DELETE: api/OrderItems/order/{orderId}
        // Delete all items for an order
        // ============================================
        /// <summary>
        /// Delete all items for a specific order
        /// </summary>
        /// <param name="orderId">The order ID</param>
        [HttpDelete("order/{orderId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAllByOrderId(int orderId)
        {
            // Note: This method is not in the current service interface
            // You would need to add it if you want this functionality
            var items = await _orderItemService.GetItemsByOrderIdAsync(orderId);
            foreach (var item in items)
            {
                await _orderItemService.DeleteAsync(item.Id);
            }
            return NoContent();
        }

        // ============================================
        // GET: api/OrderItems/{id}/exists
        // Check if an order item exists
        // ============================================
        /// <summary>
        /// Check if an order item exists
        /// </summary>
        /// <param name="id">The order item ID</param>
        [HttpGet("{id:int}/exists")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> Exists(int id)
        {
            var exists = await _orderItemService.ExistsAsync(id);
            return Ok(exists);
        }
    }
}