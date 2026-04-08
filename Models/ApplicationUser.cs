using Microsoft.AspNetCore.Identity;

namespace Twinstaranimation_backend.API.Models;

// Represents a user in the system.
// Extends IdentityUser with additional profile data and relationships.
public class ApplicationUser : IdentityUser
{
    // User profile information with users first and last name.
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    // Timestamp when the account was created
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Products created by this user (Creator role) and series they have created.
    public List<Product> Products { get; set; } = new();
    public List<Series> Series { get; set; } = new();
}
