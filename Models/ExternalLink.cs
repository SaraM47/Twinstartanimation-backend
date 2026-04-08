namespace Twinstaranimation_backend.API.Models;

// Represents an external resource linked to a chapter.
public class ExternalLink
{
    public int Id { get; set; }

    // Display title for the link and the URL to the external resource (e.g., YouTube video, SoundCloud track, etc.)
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;

    // Optional platform type (e.g., "YouTube", "Patreon") to help identify the type of content being linked.
    public string? Platform { get; set; }

    // Foreign key to Chapter and navigation to parent chapter
    public int ChapterId { get; set; }
    public Chapter Chapter { get; set; } = null!;
}
