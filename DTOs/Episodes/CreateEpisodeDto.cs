namespace Twinstaranimation_backend.API.DTOs.Episodes;

// This DTO is used when creating a new episode.
public class CreateEpisodeDto
{
    // Title of the new episode
    public string Title { get; set; } = string.Empty;

    // Position of the episode inside the series
    public int SortOrder { get; set; }

    // ID of the series the episode belongs to
    public int SeriesId { get; set; }
}
