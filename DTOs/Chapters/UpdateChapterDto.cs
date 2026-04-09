namespace Twinstaranimation_backend.API.DTOs.Chapters;

// This DTO is used when updating an existing chapter.

public class UpdateChapterDto
{
    public string Title { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
