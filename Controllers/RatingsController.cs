using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Twinstaranimation_backend.API.DTOs.Ratings;
using Twinstaranimation_backend.API.Services.Interfaces;

namespace Twinstaranimation_backend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RatingsController : ControllerBase
{
    private readonly IRatingService _ratingService;

    public RatingsController(IRatingService ratingService)
    {
        _ratingService = ratingService;
    }

    // Rate series (Customer only)
    [Authorize(Roles = "Customer")]
    [HttpPost("rate")]
    public async Task<IActionResult> Rate([FromBody] RateSeriesDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { message = "User not authenticated" });

        if (dto.SeriesId <= 0)
            return BadRequest(new { message = "Invalid series ID" });

        if (dto.Value < 1 || dto.Value > 10)
            return BadRequest(new { message = "Rating must be between 1 and 10" });

        await _ratingService.RateAsync(dto.SeriesId, userId, dto.Value);

        return Ok(new { message = "Rating submitted" });
    }

    // GET average (Public)
    [AllowAnonymous]
    [HttpGet("{seriesId}/average")]
    public async Task<IActionResult> GetAverage(int seriesId)
    {
        if (seriesId <= 0)
            return BadRequest(new { message = "Invalid series ID" });

        var avg = await _ratingService.GetAverageRatingAsync(seriesId);

        return Ok(new { seriesId, average = avg });
    }

    // GET user rating (Customer only)
    [Authorize(Roles = "Customer")]
    [HttpGet("{seriesId}/me")]
    public async Task<IActionResult> GetMyRating(int seriesId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var rating = await _ratingService.GetUserRatingAsync(seriesId, userId);

        return Ok(new { value = rating });
    }
}
