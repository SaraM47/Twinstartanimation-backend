namespace Twinstaranimation_backend.API.DTOs.Series;

// This DTO is used when creating a new series, that belongs to a product and contains chapters

public class CreateSeriesDto
{
    // ID of the product this series belongs to
    public int ProductId { get; set; }

    // Title of the series
    public string Title { get; set; } = string.Empty;

    // Description of the series
    public string Description { get; set; } = string.Empty;

    // Cover image for the series
    public string CoverImageUrl { get; set; } = string.Empty;
}
