namespace Twinstaranimation_backend.API.DTOs;

// This DTO is used when creating or updating a video, which  can belong either to a series or a chapter

public class CreateVideoDto
{
    // Title of the video
    public string Title { get; set; } = string.Empty;

    // URL of the video
    public string VideoUrl { get; set; } = string.Empty;

    // Optional: attach video to a series
    public int? SeriesId { get; set; }

    // Optional: attach video to a chapter
    public int? ChapterId { get; set; }

    // Order of the video within series or chapter
    public int SortOrder { get; set; }
}
