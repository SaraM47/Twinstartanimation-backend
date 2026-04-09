namespace Twinstaranimation_backend.API.DTOs.Orders;

// This DTO is used when a customer creates a new order (checkout), a list of items the user wants to purchase

public class CreateOrderDto
{
    // List of products included in the order
    public List<CreateOrderItemDto> Items { get; set; } = new();
}

// Represents a single item in the order request
public class CreateOrderItemDto
{
    public int ProductId { get; set; } // ID of the product being purchased
    public int Quantity { get; set; } // Quantity of the product
}
