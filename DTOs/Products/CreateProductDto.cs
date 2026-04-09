namespace Twinstaranimation_backend.API.DTOs.Products;

// This DTO is used when creating a new product

public class CreateProductDto
{
    public string Title { get; set; } = string.Empty; // Title of the new product
    public string Description { get; set; } = string.Empty; // Description of the new product
    public decimal Price { get; set; } // Price of the new product
    public string? ImageUrl { get; set; } // Optional image URL for the new product
}
