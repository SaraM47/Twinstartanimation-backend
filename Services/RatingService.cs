using Microsoft.EntityFrameworkCore;
using Twinstaranimation_backend.API.Data;
using Twinstaranimation_backend.API.Models;
using Twinstaranimation_backend.API.Services.Interfaces;

public class RatingService : IRatingService
{
    private readonly ApplicationDbContext _context;

    public RatingService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Rate (create or update)
    public async Task RateAsync(int seriesId, string userId, int value)
    {
        var existing = await _context.Ratings.FirstOrDefaultAsync(r =>
            r.SeriesId == seriesId && r.UserId == userId
        );

        if (existing != null)
        {
            existing.Value = value;
        }
        else
        {
            _context.Ratings.Add(
                new Rating
                {
                    SeriesId = seriesId,
                    UserId = userId,
                    Value = value,
                    CreatedAt = DateTime.UtcNow,
                }
            );
        }

        await _context.SaveChangesAsync();
    }

    // GET average
    public async Task<double> GetAverageRatingAsync(int seriesId)
    {
        return await _context
                .Ratings.Where(r => r.SeriesId == seriesId)
                .Select(r => (double?)r.Value)
                .AverageAsync() ?? 0;
    }

    // GET user rating
    public async Task<int?> GetUserRatingAsync(int seriesId, string userId)
    {
        return await _context
            .Ratings.Where(r => r.SeriesId == seriesId && r.UserId == userId)
            .Select(r => (int?)r.Value)
            .FirstOrDefaultAsync();
    }
}
