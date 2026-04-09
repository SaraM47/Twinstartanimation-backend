namespace Twinstaranimation_backend.API.DTOs.Orders;

// This DTO represents a single item inside an order (response), with product details and quantity

public class OrderItemDto
{
    public string ProductTitle { get; set; } = string.Empty; // Title of the product being purchased
    public decimal Price { get; set; } // Price of the product at the time of purchase
    public int Quantity { get; set; } // Quantity of the product being purchased
}
