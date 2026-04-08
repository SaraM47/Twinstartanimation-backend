using System;
using Twinstaranimation_backend.API.Models;

namespace Twinstaranimation_backend.API.Models;

// This file will represent a chapter within a series.
// Contains ordered content like pages, videos, and links.
public class Chapter
{
    public int Id { get; set; }

    // Title of the chapter
    public string Title { get; set; } = string.Empty;

    // Used to control display order inside a series
    public int SortOrder { get; set; }

    // Timestamp when the chapter was created
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign key to Series and navigationt to parent series
    public int SeriesId { get; set; }
    public Series Series { get; set; } = null!;

    // Pages belonging to this chapter (e.g. comic pages), videos (e.g. animation clips), and external links (e.g. references, related content, Youtube, links)
    public List<Page> Pages { get; set; } = new();
    public List<Video> Videos { get; set; } = new();
    public List<ExternalLink> Links { get; set; } = new();
}
