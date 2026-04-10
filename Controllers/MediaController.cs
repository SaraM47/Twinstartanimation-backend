using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Twinstaranimation_backend.API.Data;
using Twinstaranimation_backend.API.DTOs;
using Twinstaranimation_backend.API.Models;

// This controller manages all media content in the system.
// It includes Pages, Videos, and External Links.
// Only creators are allowed to create, update, or delete media.

[ApiController]
[Route("api/[controller]")]
public class MediaController : ControllerBase
{
    // Database context for accessing media data
    private readonly ApplicationDbContext _context;

    public MediaController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Helpers metods
    // Check if the user owns a specific chapter
    private async Task<bool> IsOwnerOfChapter(string userId, int chapterId)
    {
        return await _context
            .Chapters.Include(c => c.Series)
            .AnyAsync(c => c.Id == chapterId && c.Series.CreatorId == userId);
    }

    // Check if the user owns a specific series
    private async Task<bool> IsOwnerOfSeries(string userId, int seriesId)
    {
        return await _context.Series.AnyAsync(s => s.Id == seriesId && s.CreatorId == userId);
    }

    // Pages
    // Get all pages in a chapter
    [Authorize(Roles = "Creator")]
    [HttpGet("pages/chapter/{chapterId}")]
    public async Task<IActionResult> GetPages(int chapterId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Ensure ownership
        if (!await IsOwnerOfChapter(userId!, chapterId))
            return StatusCode(403, "You do not own this content");

        // Fetch pages for the chapter, ordered by page number
        var pages = await _context
            .Pages.Where(p => p.ChapterId == chapterId)
            .OrderBy(p => p.PageNumber)
            .ToListAsync();

        // Return DTOs
        return Ok(
            pages.Select(p => new PageDto
            {
                Id = p.Id,
                Title = p.Title,
                ImageUrl = p.ImageUrl,
                PageNumber = p.PageNumber,
                Content = p.Content,
                ChapterId = p.ChapterId,
            })
        );
    }

    // Create a new page
    [Authorize(Roles = "Creator")]
    [HttpPost("pages")]
    public async Task<IActionResult> CreatePage(CreatePageDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!await IsOwnerOfChapter(userId!, dto.ChapterId))
            return StatusCode(403, "You do not own this content");

        var page = new Page
        {
            Title = dto.Title,
            ImageUrl = dto.ImageUrl,
            PageNumber = dto.PageNumber,
            Content = dto.Content,
            ChapterId = dto.ChapterId,
        };

        _context.Pages.Add(page);
        await _context.SaveChangesAsync();

        return Ok(
            new PageDto
            {
                Id = page.Id,
                Title = page.Title,
                ImageUrl = page.ImageUrl,
                PageNumber = page.PageNumber,
                Content = page.Content,
                ChapterId = page.ChapterId,
            }
        );
    }

    // Update an existing page
    [Authorize(Roles = "Creator")]
    [HttpPut("pages/{id}")]
    public async Task<IActionResult> UpdatePage(int id, UpdatePageDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var page = await _context
            .Pages.Include(p => p.Chapter)
            .ThenInclude(c => c.Series)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (page == null)
            return NotFound();

        // Ensure ownership
        if (page.Chapter.Series.CreatorId != userId)
            return StatusCode(403, "You do not own this content");

        // Update fields
        page.Title = dto.Title;
        page.ImageUrl = dto.ImageUrl;
        page.PageNumber = dto.PageNumber;
        page.Content = dto.Content;

        await _context.SaveChangesAsync();

        return Ok(
            new PageDto
            {
                Id = page.Id,
                Title = page.Title,
                ImageUrl = page.ImageUrl,
                PageNumber = page.PageNumber,
                Content = page.Content,
                ChapterId = page.ChapterId,
            }
        );
    }

    // Delete a page
    [Authorize(Roles = "Creator")]
    [HttpDelete("pages/{id}")]
    public async Task<IActionResult> DeletePage(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var page = await _context
            .Pages.Include(p => p.Chapter)
            .ThenInclude(c => c.Series)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (page == null)
            return NotFound();

        if (page.Chapter.Series.CreatorId != userId)
            return StatusCode(403, "You do not own this content");

        _context.Pages.Remove(page);
        await _context.SaveChangesAsync();

        return Ok("Deleted");
    }

    // Videos
    // Get videos for a series
    [Authorize(Roles = "Creator")]
    [HttpGet("videos/series/{seriesId}")]
    public async Task<IActionResult> GetSeriesVideos(int seriesId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!await IsOwnerOfSeries(userId!, seriesId))
            return StatusCode(403, "You do not own this series");

        var videos = await _context
            .Videos.Where(v => v.SeriesId == seriesId)
            .OrderBy(v => v.SortOrder)
            .ToListAsync();

        return Ok(
            videos.Select(v => new
            {
                v.Id,
                v.Title,
                v.VideoUrl,
                v.SeriesId,
                v.ChapterId,
                v.SortOrder,
            })
        );
    }

    // Get videos for a chapter
    [Authorize(Roles = "Creator")]
    [HttpGet("videos/chapter/{chapterId}")]
    public async Task<IActionResult> GetChapterVideos(int chapterId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Validate ownership depending on where video belongs
        if (!await IsOwnerOfChapter(userId!, chapterId))
            return StatusCode(403, "You do not own this content");

        var videos = await _context
            .Videos.Where(v => v.ChapterId == chapterId)
            .OrderBy(v => v.SortOrder)
            .ToListAsync();

        return Ok(
            videos.Select(v => new
            {
                v.Id,
                v.Title,
                v.VideoUrl,
                v.SeriesId,
                v.ChapterId,
                v.SortOrder,
            })
        );
    }

    // Create a video (must belong to series or chapter)
    [Authorize(Roles = "Creator")]
    [HttpPost("videos")]
    public async Task<IActionResult> CreateVideo(CreateVideoDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Validate ownership depending on where video belongs
        if (dto.SeriesId == null && dto.ChapterId == null)
            return BadRequest("Video must belong to a Series or Chapter");

        if (dto.SeriesId != null && !await IsOwnerOfSeries(userId!, dto.SeriesId.Value))
            return StatusCode(403, "You do not own this series");

        if (dto.ChapterId != null && !await IsOwnerOfChapter(userId!, dto.ChapterId.Value))
            return StatusCode(403, "You do not own this chapter");

        var video = new Video
        {
            Title = dto.Title,
            VideoUrl = dto.VideoUrl,
            SeriesId = dto.SeriesId,
            ChapterId = dto.ChapterId,
            SortOrder = dto.SortOrder,
        };

        _context.Videos.Add(video);
        await _context.SaveChangesAsync();

        return Ok(
            new
            {
                video.Id,
                video.Title,
                video.VideoUrl,
                video.SeriesId,
                video.ChapterId,
                video.SortOrder,
            }
        );
    }

    // Update a video (must belong to series or chapter)
    [Authorize(Roles = "Creator")]
    [HttpPut("videos/{id}")]
    public async Task<IActionResult> UpdateVideo(int id, CreateVideoDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var video = await _context
            .Videos.Include(v => v.Series)
            .Include(v => v.Chapter)
            .ThenInclude(c => c.Series)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (video == null)
            return NotFound();

        if (video.SeriesId != null)
        {
            if (video.Series == null)
                return BadRequest("Invalid series reference");

            if (video.Series.CreatorId != userId)
                return StatusCode(403, "You do not own this series");
        }
        else if (video.ChapterId != null)
        {
            if (video.Chapter == null || video.Chapter.Series == null)
                return BadRequest("Invalid chapter reference");

            if (video.Chapter.Series.CreatorId != userId)
                return StatusCode(403, "You do not own this content");
        }

        video.Title = dto.Title;
        video.VideoUrl = dto.VideoUrl;
        video.SortOrder = dto.SortOrder;

        await _context.SaveChangesAsync();

        return Ok(
            new
            {
                video.Id,
                video.Title,
                video.VideoUrl,
                video.SeriesId,
                video.ChapterId,
                video.SortOrder,
            }
        );
    }

    // Delete a video
    [Authorize(Roles = "Creator")]
    [HttpDelete("videos/{id}")]
    public async Task<IActionResult> DeleteVideo(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var video = await _context
            .Videos.Include(v => v.Series)
            .Include(v => v.Chapter)
            .ThenInclude(c => c.Series)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (video == null)
            return NotFound();

        // Validate ownership depending on where video belongs
        if (video.SeriesId != null)
        {
            if (video.Series == null)
                return BadRequest("Invalid series reference");

            if (video.Series.CreatorId != userId)
                return StatusCode(403, "You do not own this series");
        }
        else if (video.ChapterId != null)
        {
            if (video.Chapter == null || video.Chapter.Series == null)
                return BadRequest("Invalid chapter reference");

            if (video.Chapter.Series.CreatorId != userId)
                return StatusCode(403, "You do not own this content");
        }

        _context.Videos.Remove(video);
        await _context.SaveChangesAsync();

        return Ok("Deleted");
    }

    // Links
    // Get external links for a chapter
    [Authorize(Roles = "Creator")]
    [HttpGet("links/chapter/{chapterId}")]
    public async Task<IActionResult> GetLinks(int chapterId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!await IsOwnerOfChapter(userId!, chapterId))
            return StatusCode(403, "You do not own this content");

        var links = await _context.ExternalLinks.Where(l => l.ChapterId == chapterId).ToListAsync();

        return Ok(
            links.Select(l => new
            {
                l.Id,
                l.Title,
                l.Url,
                l.ChapterId,
            })
        );
    }

    // Create a new external link
    [Authorize(Roles = "Creator")]
    [HttpPost("links")]
    public async Task<IActionResult> CreateLink(CreateExternalLinkDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!await IsOwnerOfChapter(userId!, dto.ChapterId))
            return StatusCode(403, "You do not own this content");

        var link = new ExternalLink
        {
            Title = dto.Title,
            Url = dto.Url,
            ChapterId = dto.ChapterId,
        };

        _context.ExternalLinks.Add(link);
        await _context.SaveChangesAsync();

        return Ok(
            new
            {
                link.Id,
                link.Title,
                link.Url,
                link.ChapterId,
            }
        );
    }

    // Delete an external link
    [Authorize(Roles = "Creator")]
    [HttpDelete("links/{id}")]
    public async Task<IActionResult> DeleteLink(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var link = await _context
            .ExternalLinks.Include(l => l.Chapter)
            .ThenInclude(c => c.Series)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (link == null)
            return NotFound();

        if (link.Chapter.Series.CreatorId != userId)
            return StatusCode(403, "You do not own this content");

        _context.ExternalLinks.Remove(link);
        await _context.SaveChangesAsync();

        return Ok("Deleted");
    }
}
