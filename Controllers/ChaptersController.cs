using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Twinstaranimation_backend.API.Data;
using Twinstaranimation_backend.API.DTOs.Chapters;
using Twinstaranimation_backend.API.Models;
using Twinstaranimation_backend.API.Services.Interfaces;

// This controller manages chapters inside a series
// It supports both customers (who purchased content) and creators (who own content)
[ApiController]
[Route("api/[controller]")]
public class ChaptersController : ControllerBase
{
    // Database context for accessing chapters and series
    private readonly ApplicationDbContext _context;

    // Service used to check if a user has access to purchased content
    private readonly IAccessService _accessService;

    // Constructor with dependency injection for database context and access service
    public ChaptersController(ApplicationDbContext context, IAccessService accessService)
    {
        _context = context;
        _accessService = accessService;
    }

    // GET chapters (Public)
    // Does no need a user to be logegd in and have pruchased the series. Now public preview (no purchase required)
    [AllowAnonymous]
    [HttpGet("series/{seriesId}")]
    public async Task<IActionResult> GetChapters(int seriesId)
    {
        // Get user ID from JWT token (optional now)
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Fetch series
        var series = await _context.Series.FirstOrDefaultAsync(s => s.Id == seriesId);

        if (series == null)
            return NotFound("Series not found");

        // Public Fetch chapters for everyone
        var chapters = await _context
            .Chapters.Where(c => c.SeriesId == seriesId)
            .OrderBy(c => c.SortOrder)
            .ToListAsync();

        // Convert to DTO (safe response)
        var result = chapters.Select(c => new ChapterDto
        {
            Id = c.Id,
            Title = c.Title,
            SortOrder = c.SortOrder,
            SeriesId = c.SeriesId,
        });

        return Ok(result);
    }

    // GET chapters (Creator)
    // Only creators can see their own content (even without purchase)
    [Authorize(Roles = "Creator")]
    [HttpGet("creator/series/{seriesId}")]
    public async Task<IActionResult> GetChaptersForCreator(int seriesId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Fetch chapters only if creator owns the series
        var chapters = await _context
            .Chapters.Where(c => c.SeriesId == seriesId && c.Series.CreatorId == userId)
            .OrderBy(c => c.SortOrder)
            .ToListAsync();

        // Convert to DTO (safe response)
        var result = chapters.Select(c => new ChapterDto
        {
            Id = c.Id,
            Title = c.Title,
            SortOrder = c.SortOrder,
            SeriesId = c.SeriesId,
        });

        return Ok(result);
    }

    // Create chapter (Creator)
    [Authorize(Roles = "Creator")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateChapterDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Find series to ensure it exists and belongs to creator
        var series = await _context.Series.FirstOrDefaultAsync(s => s.Id == dto.SeriesId);

        // If series doesn't exist, return 404
        if (series == null)
            return NotFound("Series not found");

        // Ensure creator owns the series
        if (series.CreatorId != userId)
            return StatusCode(403, "You do not own this series");

        // Create new chapter
        var chapter = new Chapter
        {
            Title = dto.Title,
            SortOrder = dto.SortOrder,
            SeriesId = dto.SeriesId,
        };

        _context.Chapters.Add(chapter);
        await _context.SaveChangesAsync();

        // Return created chapter
        return Ok(
            new ChapterDto
            {
                Id = chapter.Id,
                Title = chapter.Title,
                SortOrder = chapter.SortOrder,
                SeriesId = chapter.SeriesId,
            }
        );
    }

    // Update chapters (Creator)
    [Authorize(Roles = "Creator")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateChapterDto dto)
    {
        // Get user ID from JWT token
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Fetch chapter with related series to check ownership
        var chapter = await _context
            .Chapters.Include(c => c.Series)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (chapter == null)
            return NotFound("Chapter not found");

        // Ensure ownership
        if (chapter.Series.CreatorId != userId)
            return StatusCode(403, "You do not own this content");

        // Update fields
        chapter.Title = dto.Title;
        chapter.SortOrder = dto.SortOrder;

        await _context.SaveChangesAsync();

        return Ok(
            new ChapterDto
            {
                Id = chapter.Id,
                Title = chapter.Title,
                SortOrder = chapter.SortOrder,
                SeriesId = chapter.SeriesId,
            }
        );
    }

    // Delete chapter (Creator)
    [Authorize(Roles = "Creator")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Get chapter with related series
        var chapter = await _context
            .Chapters.Include(c => c.Series)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (chapter == null)
            return NotFound("Chapter not found");

        // Ensure ownership
        if (chapter.Series.CreatorId != userId)
            return StatusCode(403, "You do not own this content");

        _context.Chapters.Remove(chapter);
        await _context.SaveChangesAsync();

        return Ok("Deleted");
    }
}
