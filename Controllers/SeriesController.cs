using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twinstaranimation_backend.API.DTOs.Series;

// This controller manages series (collections of animation, manga, comics etc).
// Series are public for viewing, but only creators can create, update, or delete them.

[ApiController]
[Route("api/[controller]")]
public class SeriesController : ControllerBase
{
    // Service that contains business logic for series
    private readonly ISeriesService _seriesService;

    public SeriesController(ISeriesService seriesService)
    {
        _seriesService = seriesService;
    }

    // GET all series (public)
    // Returns all available series with basic information (no chapters or content details)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var series = await _seriesService.GetAllSeriesAsync();
        return Ok(series);
    }

    // GET series by ID (public)
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        // Fetch series by ID
        var series = await _seriesService.GetSeriesByIdAsync(id);

        if (series == null)
            return NotFound(new { message = "Series not found" });

        return Ok(series);
    }

    // Create new series (Creator only)
    [Authorize(Roles = "Creator")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateSeriesDto dto)
    {
        // Get creator ID from JWT
        var creatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(creatorId))
            return Unauthorized();

        // Create series via service layer
        var createdSeries = await _seriesService.CreateSeriesAsync(dto, creatorId);

        // Return 201 Created with location header
        return CreatedAtAction(nameof(GetById), new { id = createdSeries.Id }, createdSeries);
    }

    // Update series (Creator only, must own the series)
    [Authorize(Roles = "Creator")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateSeriesDto dto)
    {
        var creatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Update series (service handles ownership check)
        var updated = await _seriesService.UpdateSeriesAsync(id, dto, creatorId);

        if (!updated)
            return StatusCode(403, "You do not own this series or it does not exist");

        return Ok(new { message = "Updated" });
    }

    // Delete series (Creator only)
    [Authorize(Roles = "Creator")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var creatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Delete series (service handles ownership check)
        var deleted = await _seriesService.DeleteSeriesAsync(id, creatorId);

        if (!deleted)
            return StatusCode(403, "You do not own this series or it does not exist");

        return Ok(new { message = "Deleted" });
    }
}
