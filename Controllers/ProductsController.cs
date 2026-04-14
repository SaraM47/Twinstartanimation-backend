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

        // Find product by ID and load related data
        var product = await _context
            .Products.Include(p => p.Series)
            .ThenInclude(s => s.Chapters)
            .ThenInclude(c => c.Pages)
            .Include(p => p.Series)
            .ThenInclude(s => s.Chapters)
            .ThenInclude(c => c.Videos)
            .Include(p => p.Series)
            .ThenInclude(s => s.Chapters)
            .ThenInclude(c => c.Links)
            .Include(p => p.Series)
            .ThenInclude(s => s.Videos)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            return NotFound();

        // Ensure creator owns the product
        if (product.CreatorId != userId)
            return StatusCode(403, "You do not own this product");

        // Prevent deleting product if it exists in orders
        var hasOrders = await _context.OrderItems.AnyAsync(oi => oi.ProductId == id);

        if (hasOrders)
        {
            return BadRequest(
                new
                {
                    message = "This product cannot be deleted because it exists in one or more orders.",
                }
            );
        }

        // Use transaction to avoid partial deletes
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Delete all related content (Series → Chapters → Media)
            foreach (var series in product.Series)
            {
                // Series-level videos
                if (series.Videos.Any())
                {
                    _context.Videos.RemoveRange(series.Videos);
                }

                foreach (var chapter in series.Chapters)
                {
                    if (chapter.Pages.Any())
                    {
                        _context.Pages.RemoveRange(chapter.Pages);
                    }

                    if (chapter.Videos.Any())
                    {
                        _context.Videos.RemoveRange(chapter.Videos);
                    }

                    if (chapter.Links.Any())
                    {
                        _context.ExternalLinks.RemoveRange(chapter.Links);
                    }
                }

                if (series.Chapters.Any())
                {
                    _context.Chapters.RemoveRange(series.Chapters);
                }
            }

            if (product.Series.Any())
            {
                _context.Series.RemoveRange(product.Series);
            }

            // Remove product from database
            _context.Products.Remove(product);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // Return success message
            return Ok(new { message = "Deleted" });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            return StatusCode(
                500,
                new
                {
                    message = "An error occurred while deleting the product.",
                    details = ex.Message,
                }
            );
        }
    }
}
