namespace Twinstaranimation_backend.API.DTOs.Products;

// This DTO is used when updating an existing product

public class UpdateProductDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}
