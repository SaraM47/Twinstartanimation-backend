namespace Twinstaranimation_backend.API.DTOs.Series;

// This DTO is used when returning series data to the client

public class SeriesDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public string? CreatorId { get; set; }
    public DateTime CreatedAt { get; set; }

    public string? Authors { get; set; }
    public string? Status { get; set; }
    public string? Genres { get; set; }

    public double AverageRating { get; set; }
}
