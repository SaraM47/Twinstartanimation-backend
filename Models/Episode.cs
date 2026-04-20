using System;
using Twinstaranimation_backend.API.Models;

namespace Twinstaranimation_backend.API.Models;

// Represents an episode inside an animation series.
// Used separately from Chapters (which are for comics/manga).
public class Episode
{
    public int Id { get; set; }

    // Episode title
    public string Title { get; set; } = string.Empty;

    // Used to control display order inside a series
    public int SortOrder { get; set; }

    // Timestamp when the episode was created
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign key to Series
    public int SeriesId { get; set; }
    public Series Series { get; set; } = null!;

    // Videos belonging to this episode
    public List<Video> Videos { get; set; } = new();
}
