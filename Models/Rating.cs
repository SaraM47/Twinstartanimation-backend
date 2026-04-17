namespace Twinstaranimation_backend.API.Models;

public class Rating
{
    public int Id { get; set; }

    public int SeriesId { get; set; }
    public Series? Series { get; set; }

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }

    public int Value { get; set; } // 1–10

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
