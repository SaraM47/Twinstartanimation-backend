namespace Twinstaranimation_backend.API.Models;

// Represents a single page inside a chapter (e.g. comic or text content).
public class Page
{
    public int Id { get; set; }

    // Optional page title
    public string? Title { get; set; }

    // Image URL for visual content (e.g. manga/comics)
    public string ImageUrl { get; set; } = string.Empty;

    // Page order within the chapter
    public int PageNumber { get; set; }

    // Optional text content
    public string? Content { get; set; }

    // FK to Chapter and navigation to parent chapter
    public int ChapterId { get; set; }
    public Chapter Chapter { get; set; } = null!;
}
