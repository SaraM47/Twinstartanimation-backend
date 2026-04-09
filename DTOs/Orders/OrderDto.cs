namespace Twinstaranimation_backend.API.DTOs.Orders;

// This DTO is used when returning order data to the client, it includes order details and a list of purchased items

public class OrderDto
{
    public int Id { get; set; } // Unique ID for the order
    public decimal TotalAmount { get; set; } // Total cost of the order
    public string Status { get; set; } = string.Empty; // Current status of the order (e.g., "Pending", "Completed", "Cancelled")

    public List<OrderItemDto> Items { get; set; } = new(); // List of items included in the order, with product details and quantity
}
