namespace Twinstaranimation_backend.API.Models;

// Represents a video that can belong to either a series, chapter, or episode.
public class Video
{
    public int Id { get; set; }

    // Video title and URL (required for both series and chapter videos)
    public string Title { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;

    // Optional link to Series (for animations)
    public int? SeriesId { get; set; }
    public Series? Series { get; set; }

    // Optional link to Chapter (hybrid)
    public int? ChapterId { get; set; }
    public Chapter? Chapter { get; set; }

    // Optional link to Episode (for animation episodes)
    public int? EpisodeId { get; set; }
    public Episode? Episode { get; set; }

    // Sort order for videos within a series, chapter, or episode
    public int SortOrder { get; set; }

    // Timestamp when the video was created
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
