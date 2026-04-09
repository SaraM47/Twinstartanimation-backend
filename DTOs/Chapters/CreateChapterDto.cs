namespace Twinstaranimation_backend.API.DTOs.Chapters;

// This DTO is used when creating a new chapter.

public class CreateChapterDto
{
    public string Title { get; set; } = string.Empty; // Title of the new chapter
    public int SortOrder { get; set; } // Position of the chapter in the series
    public int SeriesId { get; set; } // ID of the series the chapter will belong to
}
