namespace Twinstaranimation_backend.API.DTOs.Auth;

// This DTO is used when a user logs in, which contains the data sent from the client to authenticate the user.

public class LoginDto
{
    // User's email address (used as username)
    public string Email { get; set; } = string.Empty;

    // User's password (plain text from client, validated by backend)
    public string Password { get; set; } = string.Empty;
}
