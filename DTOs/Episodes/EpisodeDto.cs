namespace Twinstaranimation_backend.API.DTOs.Episodes;

// Safe response DTO for returning episode data
public class EpisodeDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public int SeriesId { get; set; }

    public DateTime CreatedAt { get; set; }
}
