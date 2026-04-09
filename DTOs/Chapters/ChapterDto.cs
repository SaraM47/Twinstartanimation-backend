namespace Twinstaranimation_backend.API.DTOs.Chapters;

// This DTO is used when returning chapter data to the client

public class ChapterDto
{
    public int Id { get; set; } // Unique identifier for the chapter
    public string Title { get; set; } = string.Empty; // Title of the chapter
    public int SortOrder { get; set; } // Order of the chapter within the series
    public int SeriesId { get; set; } // Foreign key to the series this chapter belongs to
}
