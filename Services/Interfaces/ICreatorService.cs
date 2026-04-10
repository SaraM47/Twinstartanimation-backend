// This interface defines functionality for creator analytics.
// It provides dashboard data such as revenue and sales.

public interface ICreatorService
{
    // Returns dashboard statistics for a creator
    Task<CreatorDashboardDto> GetDashboardAsync(string creatorId);
}
