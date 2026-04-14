using Microsoft.EntityFrameworkCore;
using Twinstaranimation_backend.API.Data;

// This service calculates creator dashboard statistics

public class CreatorService : ICreatorService
{
    private readonly ApplicationDbContext _context;

    public CreatorService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Get dashboard metrics for a creator
    public async Task<CreatorDashboardDto> GetDashboardAsync(string creatorId)
    {
        // Get all paid order items belonging to this creator
        // Query order items for products created by this creator, only for paid orders
        var orderItemsQuery = _context
            .OrderItems.Include(oi => oi.Product)
            .Include(oi => oi.Order)
            .Where(oi => oi.Product.CreatorId == creatorId && oi.Order.Status == "Paid");

        // Calculate metrics
        var totalRevenue = await orderItemsQuery.SumAsync(oi => oi.Price * oi.Quantity);

        var totalSales = await orderItemsQuery.SumAsync(oi => oi.Quantity);

        var totalOrders = await orderItemsQuery.Select(oi => oi.OrderId).Distinct().CountAsync();

        var totalProducts = await _context.Products.CountAsync(p => p.CreatorId == creatorId);

        var totalSeries = await _context.Series.CountAsync(s => s.CreatorId == creatorId);

        var totalChapters = await _context
            .Chapters.Where(c => c.Series.CreatorId == creatorId)
            .CountAsync();

        // Return the metrics as a DTO
        return new CreatorDashboardDto
        {
            TotalRevenue = totalRevenue,
            TotalSales = totalSales,
            TotalOrders = totalOrders,
            TotalProducts = totalProducts,
            TotalSeries = totalSeries,
            TotalChapters = totalChapters,
        };
    }
}
