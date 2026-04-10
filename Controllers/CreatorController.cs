using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// This controller handles features specifically for creators.
// It currently provides access to creator dashboard data.
[ApiController]
[Route("api/[controller]")]
public class CreatorController : ControllerBase
{
    // Service that contains business logic for creator-related data (e.g., earnings, sales)
    private readonly ICreatorService _creatorService;

    // Constructor with dependency injection
    public CreatorController(ICreatorService creatorService)
    {
        _creatorService = creatorService;
    }

    // GET creator dashboard data
    // Returns statistics such as total earnings, total sales, etc.
    // Only users with "Creator" role are allowed
    [Authorize(Roles = "Creator")]
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        // Get creator ID from JWT token
        var creatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // If no user ID found, return unauthorized
        if (string.IsNullOrEmpty(creatorId))
            return Unauthorized();

        // Fetch dashboard data from service layer
        var result = await _creatorService.GetDashboardAsync(creatorId);

        // Return the dashboard data to the client
        return Ok(result);
    }
}
