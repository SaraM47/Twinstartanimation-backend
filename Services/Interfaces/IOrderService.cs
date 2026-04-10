using Twinstaranimation_backend.API.DTOs.Orders;
using Twinstaranimation_backend.API.Models;

// This interface defines order-related operations such as creating an order, retrieving a user's orders, and getting order details. It is used to manage orders in the application.

public interface IOrderService
{
    // Creates a new order from input DTO
    Task<OrderDto> CreateOrderAsync(CreateOrderDto dto, string userId);

    // Returns all paid orders for a specific user
    Task<List<OrderDto>> GetMyOrdersAsync(string userId);

    // Returns full order with items (used for Stripe checkout)
    Task<Order> GetOrderWithItemsAsync(int orderId);
}
