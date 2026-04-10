using Microsoft.EntityFrameworkCore;
using Twinstaranimation_backend.API.Data;
using Twinstaranimation_backend.API.Services.Interfaces;

// This service checks if a user has access to content.
// Access is based on completed (paid) orders.

namespace Twinstaranimation_backend.API.Services;

public class AccessService : IAccessService
{
    private readonly ApplicationDbContext _context;

    public AccessService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Chech product access
    // Returns true if user has purchased a product
    public async Task<bool> HasUserPurchasedProduct(string userId, int productId)
    {
        return await _context
            .OrderItems.Where(oi => oi.ProductId == productId)
            .Join(
                _context.Orders.Where(o => o.UserId == userId && o.Status == "Paid"),
                oi => oi.OrderId,
                o => o.Id,
                (oi, o) => oi
            )
            .AnyAsync();
    }

    // Check series access
    // A user gets access to a series if they bought its product
    public async Task<bool> HasAccessToSeries(string userId, int seriesId)
    {
        var productId = await _context
            .Series.Where(s => s.Id == seriesId)
            .Select(s => s.ProductId)
            .FirstOrDefaultAsync();

        if (productId == null)
            return false;

        return await HasUserPurchasedProduct(userId, productId.Value);
    }
}
