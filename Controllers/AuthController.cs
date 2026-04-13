using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Twinstaranimation_backend.API.DTOs.Auth;
using Twinstaranimation_backend.API.Models;

// This controller handles authentication and user management.
// It provides endpoints for registering, logging in, and assigning roles.
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // UserManager is used to manage users (create, find, validate password, roles)
    private readonly UserManager<ApplicationUser> _userManager;

    // IConfiguration is used to read settings (e.g., JWT key from appsettings.json)
    private readonly IConfiguration _configuration;

    // Constructor with dependency injection
    public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    // Register new user
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(user, "Customer");

        return Ok(new { message = "User created", userId = user.Id });
    }

    // Login user
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
            return Unauthorized("Invalid email");

        var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!passwordValid)
            return Unauthorized("Invalid password");

        var keyString = _configuration["Jwt:Key"];

        if (string.IsNullOrEmpty(keyString))
            return StatusCode(500, "JWT key missing");

        var key = Encoding.UTF8.GetBytes(keyString);

        var creds = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256
        );

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        // Added HttpOnly cookie
        Response.Cookies.Append(
            "token",
            jwt,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(7),
            }
        );

        // Returnera endast info (inte token)
        return Ok(
            new
            {
                message = "Logged in",
                roles,
                userId = user.Id,
            }
        );
    }

    // Get current user info
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return Unauthorized();

        var roles = await _userManager.GetRolesAsync(user);

        return Ok(
            new
            {
                userId = user.Id,
                email = user.Email,
                roles,
            }
        );
    }

    // Logout
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(
            "token",
            new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
            }
        );

        return Ok(new { message = "Logged out" });
    }

    // Make user a creator by ID
    [HttpPost("make-creator/{userId}")]
    public async Task<IActionResult> MakeCreator(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return NotFound("User not found");

        var result = await _userManager.AddToRoleAsync(user, "Creator");

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("User is now Creator");
    }

    // Make a user a creator by email (alternative endpoint optionally)
    [HttpPost("make-creator-by-email/{email}")]
    public async Task<IActionResult> MakeCreatorByEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return NotFound("User not found");

        var result = await _userManager.AddToRoleAsync(user, "Creator");

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("User is now Creator");
    }
}
