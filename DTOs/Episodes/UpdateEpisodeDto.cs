namespace Twinstaranimation_backend.API.DTOs.Episodes;

// This DTO is used when updating an existing episode.
public class UpdateEpisodeDto
{
    // Updated title
    public string Title { get; set; } = string.Empty;

    // Updated order
    public int SortOrder { get; set; }
}
