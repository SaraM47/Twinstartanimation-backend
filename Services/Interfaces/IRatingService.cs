using Twinstaranimation_backend.API.Models;

public interface IRatingService
{
    Task RateAsync(int seriesId, string userId, int value);
    Task<double> GetAverageRatingAsync(int seriesId);
    Task<int?> GetUserRatingAsync(int seriesId, string userId);
}
