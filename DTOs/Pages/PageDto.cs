namespace Twinstaranimation_backend.API.DTOs;

// This DTO is used when returning page data to the client

public class PageDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int PageNumber { get; set; }
    public string? Content { get; set; }
    public int ChapterId { get; set; }
}
