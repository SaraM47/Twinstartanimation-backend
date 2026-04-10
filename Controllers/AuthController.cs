using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
        // Create a new user object from incoming data
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
        };

        // Try to create user with password
        var result = await _userManager.CreateAsync(user, dto.Password);

        // If creation fails, return validation errors
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        // Assign default role "Customer"
        await _userManager.AddToRoleAsync(user, "Customer");

        // Return success response
        return Ok(new { message = "User created", userId = user.Id });
    }

    // Login user
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        // Find user by email
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
            return Unauthorized("Invalid email");

        // Check if password is correct
        var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!passwordValid)
            return Unauthorized("Invalid password");

        // Get JWT secret key from configuration
        var keyString = _configuration["Jwt:Key"];

        if (string.IsNullOrEmpty(keyString))
            return StatusCode(500, "JWT key missing");

        var key = Encoding.UTF8.GetBytes(keyString);

        // Create signing credentials for token
        var creds = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256
        );

        // Get user roles (e.g., Customer, Creator)
        var roles = await _userManager.GetRolesAsync(user);

        // Create claims (data stored inside JWT)
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
        };

        // Add roles to claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Create JWT token
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        // Return token and user info
        return Ok(
            new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                roles,
                userId = user.Id,
            }
        );
    }

    // Make user a creator by ID
    [HttpPost("make-creator/{userId}")]
    public async Task<IActionResult> MakeCreator(string userId)
    {
        // Find user by ID
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return NotFound("User not found");

        // Add "Creator" role to user
        var result = await _userManager.AddToRoleAsync(user, "Creator");

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok("User is now Creator");
    }

    // Make a user a creator by email (alternative endpoint optionally)
    [HttpPost("make-creator-by-email/{email}")]
    public async Task<IActionResult> MakeCreatorByEmail(string email)
    {
        // Find user by email
        var user = await _userManager.FindByEmailAsync(email);

        // If user not found, return 404
        if (user == null)
            return NotFound("User not found");

        // Add "Creator" role to user
        var result = await _userManager.AddToRoleAsync(user, "Creator");

        // If adding role fails, return validation errors
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        // If successful, return success message
        return Ok("User is now Creator");
    }
}
