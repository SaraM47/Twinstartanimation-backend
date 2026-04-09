namespace Twinstaranimation_backend.API.DTOs.Auth;

// This DTO is used when a new user registers, which contains all required data to create a new account.

public class RegisterDto
{
    // User's email address (will be used as login username)
    public string Email { get; set; } = string.Empty;

    // User's password (will be hashed and stored securely)
    public string Password { get; set; } = string.Empty;

    // User's first name (profile information)
    public string FirstName { get; set; } = string.Empty;

    // User's last name (profile information)
    public string LastName { get; set; } = string.Empty;
}
