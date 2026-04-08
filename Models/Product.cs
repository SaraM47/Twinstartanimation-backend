namespace Twinstaranimation_backend.API.Models;

// Represents a purchasable product in the system.
public class Product
{
    public int Id { get; set; }

    // Product title and description
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Price of the product
    public decimal Price { get; set; }

    // Optional product image
    public string? ImageUrl { get; set; }

    // Timestamp for when the product was created
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // ID of the creator (owner of the product) and navigation property to the creator user
    public string CreatorId { get; set; } = string.Empty;
    public ApplicationUser Creator { get; set; } = null!;

    // Series included in this product
    public List<Series> Series { get; set; } = new();
}
