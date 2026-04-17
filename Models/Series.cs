namespace Twinstaranimation_backend.API.Models;

// Represents a content series (e.g. animation series and comics collection).
public class Series
{
    public int Id { get; set; }

    // Series title with description and cover image URL for display purposes.
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;

    // Indicates if the series is published and visible to users
    public bool IsPublished { get; set; } = false;

    // Optional link to a product (for paid content)
    public int? ProductId { get; set; }
    public Product? Product { get; set; }

    // Optional creator
    public string? CreatorId { get; set; }
    public ApplicationUser? Creator { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Chapters inside the series
    public List<Chapter> Chapters { get; set; } = new();

    // Videos attached at series level (e.g. animations)
    public List<Video> Videos { get; set; } = new();

    // Fields for add, status and genres to be used only for comics
    public string? Authors { get; set; }
    public string? Status { get; set; }
    public string? Genres { get; set; }
}
