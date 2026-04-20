using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Twinstaranimation_backend.API.Data;
using Twinstaranimation_backend.API.DTOs.Episodes;
using Twinstaranimation_backend.API.Models;

namespace Twinstaranimation_backend.API.Controllers;

// This controller manages episodes inside animation series
[ApiController]
[Route("api/[controller]")]
public class EpisodesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EpisodesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET episodes (Public)
    [AllowAnonymous]
    [HttpGet("series/{seriesId}")]
    public async Task<IActionResult> GetEpisodes(int seriesId)
    {
        var series = await _context.Series.FirstOrDefaultAsync(s => s.Id == seriesId);

        if (series == null)
            return NotFound("Series not found");

        var episodes = await _context
            .Set<Episode>()
            .Where(e => e.SeriesId == seriesId)
            .OrderBy(e => e.SortOrder)
            .ToListAsync();

        var result = episodes.Select(e => new EpisodeDto
        {
            Id = e.Id,
            Title = e.Title,
            SortOrder = e.SortOrder,
            SeriesId = e.SeriesId,
            CreatedAt = e.CreatedAt,
        });

        return Ok(result);
    }

    // GET episodes (Creator)
    [Authorize(Roles = "Creator")]
    [HttpGet("creator/series/{seriesId}")]
    public async Task<IActionResult> GetEpisodesForCreator(int seriesId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var episodes = await _context
            .Set<Episode>()
            .Where(e => e.SeriesId == seriesId && e.Series.CreatorId == userId)
            .OrderBy(e => e.SortOrder)
            .ToListAsync();

        var result = episodes.Select(e => new EpisodeDto
        {
            Id = e.Id,
            Title = e.Title,
            SortOrder = e.SortOrder,
            SeriesId = e.SeriesId,
            CreatedAt = e.CreatedAt,
        });

        return Ok(result);
    }

    // Create episode (Creator)
    [Authorize(Roles = "Creator")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateEpisodeDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var series = await _context.Series.FirstOrDefaultAsync(s => s.Id == dto.SeriesId);

        if (series == null)
            return NotFound("Series not found");

        if (series.CreatorId != userId)
            return StatusCode(403, "You do not own this series");

        var episode = new Episode
        {
            Title = dto.Title,
            SortOrder = dto.SortOrder,
            SeriesId = dto.SeriesId,
        };

        _context.Add(episode);
        await _context.SaveChangesAsync();

        return Ok(
            new EpisodeDto
            {
                Id = episode.Id,
                Title = episode.Title,
                SortOrder = episode.SortOrder,
                SeriesId = episode.SeriesId,
                CreatedAt = episode.CreatedAt,
            }
        );
    }

    // Update episode (Creator)
    [Authorize(Roles = "Creator")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateEpisodeDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var episode = await _context
            .Set<Episode>()
            .Include(e => e.Series)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (episode == null)
            return NotFound("Episode not found");

        if (episode.Series.CreatorId != userId)
            return StatusCode(403, "You do not own this content");

        episode.Title = dto.Title;
        episode.SortOrder = dto.SortOrder;

        await _context.SaveChangesAsync();

        return Ok(
            new EpisodeDto
            {
                Id = episode.Id,
                Title = episode.Title,
                SortOrder = episode.SortOrder,
                SeriesId = episode.SeriesId,
                CreatedAt = episode.CreatedAt,
            }
        );
    }

    // Delete episode (Creator)
    [Authorize(Roles = "Creator")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var episode = await _context
            .Set<Episode>()
            .Include(e => e.Series)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (episode == null)
            return NotFound("Episode not found");

        if (episode.Series.CreatorId != userId)
            return StatusCode(403, "You do not own this content");

        _context.Remove(episode);
        await _context.SaveChangesAsync();

        return Ok("Deleted");
    }
}
