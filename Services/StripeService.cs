using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using Twinstaranimation_backend.API.Models;

// This service handles Stripe payment integration, which creates a checkout session for an order

public class StripeService : IStripeService
{
    private readonly StripeSettings _settings;

    public StripeService(IOptions<StripeSettings> settings)
    {
        // Set Stripe API key
        _settings = settings.Value;
        StripeConfiguration.ApiKey = _settings.SecretKey;
    }

    public async Task<string> CreateCheckoutSessionAsync(Order order)
    {
        // Convert order items into Stripe line items
        var lineItems = order
            .Items.Select(item => new SessionLineItemOptions
            {
                Quantity = item.Quantity,
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "sek",
                    UnitAmount = (long)(item.Price * 100),
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.Product.Title,
                    },
                },
            })
            .ToList();

        // Create Stripe session
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },

            // Link Stripe payment to order
            Metadata = new Dictionary<string, string> { { "orderId", order.Id.ToString() } },

            // Pass the line items to Stripe
            LineItems = lineItems,

            // Set the mode to payment for one-time purchases
            Mode = "payment",

            // Redirect URLs after payment success or cancellation
            SuccessUrl = "http://localhost:5174/success",
            CancelUrl = "http://localhost:5174/cancel",
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return session.Url!;
    }
}
