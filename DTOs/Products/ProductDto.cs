namespace Twinstaranimation_backend.API.DTOs.Products;

// This DTO is used when returning product data to the client

public class ProductDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
