namespace Twinstaranimation_backend.API.Models;

// Represents a single product inside an order.
public class OrderItem
{
    public int Id { get; set; }

    // FK to Order and navigation to Order
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;

    // FK to Product and navigation to Product
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    // Quantity of this product
    public int Quantity { get; set; }

    // Price per unit at time of purchase
    public decimal Price { get; set; }
}
