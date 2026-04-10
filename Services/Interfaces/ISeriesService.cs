using Twinstaranimation_backend.API.DTOs.Series;

// This interface defines operations for managing series in the application, including creating, retrieving, updating, and deleting series. It also includes access control based on the creator's ID.

public interface ISeriesService
{
    Task<SeriesDto> CreateSeriesAsync(CreateSeriesDto dto, string? creatorId);
    Task<IEnumerable<SeriesDto>> GetAllSeriesAsync();
    Task<SeriesDto?> GetSeriesByIdAsync(int id);
    Task<bool> UpdateSeriesAsync(int id, UpdateSeriesDto dto, string? creatorId);
    Task<bool> DeleteSeriesAsync(int id, string? creatorId);
}
