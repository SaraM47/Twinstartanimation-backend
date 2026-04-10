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
        var orderItems = await _context
            .OrderItems.Include(oi => oi.Product)
            .Include(oi => oi.Order)
            .Where(oi => oi.Product.CreatorId == creatorId && oi.Order.Status == "Paid")
            .ToListAsync();

        // Calculate metrics
        var totalRevenue = orderItems.Sum(oi => oi.Price * oi.Quantity);
        var totalSales = orderItems.Sum(oi => oi.Quantity);
        var totalOrders = orderItems.Select(oi => oi.OrderId).Distinct().Count();

        // Return the metrics as a DTO
        return new CreatorDashboardDto
        {
            TotalRevenue = totalRevenue,
            TotalSales = totalSales,
            TotalOrders = totalOrders,
        };
    }
}
