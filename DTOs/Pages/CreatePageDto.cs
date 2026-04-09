namespace Twinstaranimation_backend.API.DTOs;

// This DTO is used when creating a new page inside a chapter

public class CreatePageDto
{
    // Title of the page
    public string Title { get; set; } = string.Empty;

    // Text content of the page (can be description or body text)
    public string Content { get; set; } = string.Empty;

    // URL to the image displayed on the page
    public string ImageUrl { get; set; } = string.Empty;

    // Order of the page within the chapter
    public int PageNumber { get; set; }

    // ID of the chapter this page belongs to
    public int ChapterId { get; set; }
}
