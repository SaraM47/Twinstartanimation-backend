using Microsoft.EntityFrameworkCore;
using Twinstaranimation_backend.API.Data;
using Twinstaranimation_backend.API.DTOs.Series;
using Twinstaranimation_backend.API.Models;

// This service handles all logic related to Series.
// It manages creating, retrieving, updating, and deleting series.

public class SeriesService : ISeriesService
{
    // Datebase access
    private readonly ApplicationDbContext _context;

    public SeriesService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Create a new series
    // Creates a new series and assigns it to a creator and product
    public async Task<SeriesDto> CreateSeriesAsync(CreateSeriesDto dto, string? creatorId)
    {
        // Ensure creator exists
        if (string.IsNullOrEmpty(creatorId))
            throw new Exception("CreatorId is required");

        // Create new series entity
        var series = new Series
        {
            Title = dto.Title,
            Description = dto.Description,
            CoverImageUrl = dto.CoverImageUrl,

            // Controll access
            ProductId = dto.ProductId,

            // Ownership
            CreatorId = creatorId,

            CreatedAt = DateTime.UtcNow,
        };

        _context.Series.Add(series);
        await _context.SaveChangesAsync();

        // Return the created series as a DTO
        return new SeriesDto
        {
            Id = series.Id,
            Title = series.Title,
            Description = series.Description,
            CoverImageUrl = series.CoverImageUrl,
            CreatorId = series.CreatorId,
            CreatedAt = series.CreatedAt,
        };
    }

    // GET all series (public)
    // Returns all series as DTOs
    public async Task<IEnumerable<SeriesDto>> GetAllSeriesAsync()
    {
        return await _context
            .Series.Select(s => new SeriesDto
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                CoverImageUrl = s.CoverImageUrl,
                CreatorId = s.CreatorId,
                CreatedAt = s.CreatedAt,
            })
            .ToListAsync();
    }

    // GET series by ID (public)
    // Returns a single series or null
    public async Task<SeriesDto?> GetSeriesByIdAsync(int id)
    {
        return await _context
            .Series.Where(s => s.Id == id)
            .Select(s => new SeriesDto
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                CoverImageUrl = s.CoverImageUrl,
                CreatorId = s.CreatorId,
                CreatedAt = s.CreatedAt,
            })
            .FirstOrDefaultAsync();
    }

    // Update series (creator only)
    public async Task<bool> UpdateSeriesAsync(int id, UpdateSeriesDto dto, string? creatorId)
    {
        var series = await _context.Series.FindAsync(id);

        if (series == null || series.CreatorId != creatorId)
            return false;

        series.Title = dto.Title;
        series.Description = dto.Description;
        series.CoverImageUrl = dto.CoverImageUrl;

        await _context.SaveChangesAsync();
        return true;
    }

    // Delete series (creator only)
    public async Task<bool> DeleteSeriesAsync(int id, string? creatorId)
    {
        var series = await _context.Series.FindAsync(id);

        if (series == null || series.CreatorId != creatorId)
            return false;

        _context.Series.Remove(series);
        await _context.SaveChangesAsync();
        return true;
    }
}
