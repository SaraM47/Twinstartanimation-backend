using Microsoft.EntityFrameworkCore;
using Twinstaranimation_backend.API.Data;
using Twinstaranimation_backend.API.DTOs.Orders;
using Twinstaranimation_backend.API.Models;

// This service handles order creation and retrieval, calculates totals and ensures valid products

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Create order
    // Builds order with multiple products
    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto, string userId)
    {
        // Get all product IDs from request
        var productIds = dto.Items.Select(i => i.ProductId).ToList();
        // Load products from database
        var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

        // Validate all products exist
        if (products.Count != productIds.Count)
            throw new Exception("One or more products not found");

        var order = new Order
        {
            UserId = userId,
            Items = new List<OrderItem>(),
            CreatedAt = DateTime.UtcNow,
        };

        decimal total = 0;

        // Build order items
        foreach (var item in dto.Items)
        {
            var product = products.First(p => p.Id == item.ProductId);

            var orderItem = new OrderItem
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                Price = product.Price,
            };

            total += product.Price * item.Quantity;

            order.Items.Add(orderItem);
        }

        order.TotalAmount = total;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return new OrderDto
        {
            Id = order.Id,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
        };
    }

    // GET order with items
    // Used for Stripe checkout
    public async Task<Order> GetOrderWithItemsAsync(int orderId)
    {
        return await _context
            .Orders.Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .FirstAsync(o => o.Id == orderId);
    }

    // GET user order
    // Returns only paid orders
    public async Task<List<OrderDto>> GetMyOrdersAsync(string userId)
    {
        // Load all paid orders for the user, including items and product details
        var orders = await _context
            .Orders.Where(o => o.UserId == userId && o.Status == "Paid")
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .ToListAsync();

        return orders
            .Select(o => new OrderDto
            {
                Id = o.Id,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CreatedAt = o.CreatedAt,

                Items = o
                    .Items.Select(i => new OrderItemDto
                    {
                        ProductTitle = i.Product.Title,
                        Price = i.Price,
                        Quantity = i.Quantity,
                    })
                    .ToList(),
            })
            .ToList();
    }
}
