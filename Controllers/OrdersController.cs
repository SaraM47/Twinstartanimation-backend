using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twinstaranimation_backend.API.DTOs.Orders;

// This controller handles order-related actions.
// It allows customers to create orders (checkout) and view their own orders.

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    // Service for handling order logic (create orders, fetch orders)
    private readonly IOrderService _orderService;

    // Service for handling Stripe payments (checkout sessions)
    private readonly IStripeService _stripeService;

    // Constructor with dependency injection
    public OrdersController(IOrderService orderService, IStripeService stripeService)
    {
        _orderService = orderService;
        _stripeService = stripeService;
    }

    // Checkout (multi-product order creation)
    // Creates an order and generates a Stripe checkout session
    [Authorize(Roles = "Customer")]
    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout(CreateOrderDto dto)
    {
        // Get user ID from JWT token
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        // Create order in database
        var order = await _orderService.CreateOrderAsync(dto, userId);

        // Load full order with items and product details
        var fullOrder = await _orderService.GetOrderWithItemsAsync(order.Id);

        // Create Stripe checkout session
        var checkoutUrl = await _stripeService.CreateCheckoutSessionAsync(fullOrder);

        // Return order info and Stripe payment link
        return Ok(
            new
            {
                orderId = fullOrder.Id,
                totalAmount = fullOrder.TotalAmount,
                checkoutUrl,
            }
        );
    }

    // GET my orders
    // Returns all orders for the logged-in user
    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyOrders()
    {
        // Get user ID from JWT token
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // If no user ID found, return unauthorized
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        // Fetch user's orders
        var orders = await _orderService.GetMyOrdersAsync(userId);

        return Ok(orders);
    }
}
