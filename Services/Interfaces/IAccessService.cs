namespace Twinstaranimation_backend.API.Services.Interfaces;

// This interface defines methods for access control related to products and series. It checks if a user has purchased a product or has access to a series.
public interface IAccessService
{
    // Checks if a user has purchased a specific product
    Task<bool> HasUserPurchasedProduct(string userId, int productId);

    // Checks if a user has access to a series (via purchased product)
    Task<bool> HasAccessToSeries(string userId, int seriesId);
}
