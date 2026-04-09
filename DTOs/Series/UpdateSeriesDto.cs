namespace Twinstaranimation_backend.API.DTOs.Series;

// This DTO is used when updating an existing series

public class UpdateSeriesDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
}
