namespace Twinstaranimation_backend.API.DTOs;

// This DTO is used when updating an existing page

public class UpdatePageDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int PageNumber { get; set; }
}
