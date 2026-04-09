using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Twinstaranimation_backend.API.Data;
using Twinstaranimation_backend.API.DTOs.Products;
using Twinstaranimation_backend.API.Models;

// This controller manages products in the system.
// Products are public for viewing, but only creators can create, update, or delete them.

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    // Database context for accessing product data
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET all products (public)
    // Anyone can view all products
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // Fetch all products from the database
        var products = await _context.Products.ToListAsync();

        // Return the list of products to the client
        return Ok(products);
    }

    // GET product by ID (public)
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        // Find product by ID
        var product = await _context.Products.FindAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    // Create product
    // Only creators can create products
    [Authorize(Roles = "Creator")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        // Get creator ID from JWT
        var creatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Create new product
        var product = new Product
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            ImageUrl = dto.ImageUrl,
            CreatorId = creatorId!,
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return Ok(product);
    }

    // Update product
    [Authorize(Roles = "Creator")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return NotFound();

        // Ensure creator owns the product
        if (product.CreatorId != userId)
            return StatusCode(403, "You do not own this product");

        product.Title = dto.Title;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.ImageUrl = dto.ImageUrl;

        await _context.SaveChangesAsync();

        return Ok(product);
    }

    // Delete product
    [Authorize(Roles = "Creator")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Find product by ID
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return NotFound();

        if (product.CreatorId != userId)
            return StatusCode(403, "You do not own this product");

        // Remove product from database
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        // Return success message
        return Ok(new { message = "Deleted" });
    }
}
