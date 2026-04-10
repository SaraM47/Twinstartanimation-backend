using Twinstaranimation_backend.API.Models;

// This interface defines Stripe payment functionality for creating a checkout session based on an order. It is used to create a checkout session for an order.
public interface IStripeService
{
    // Creates a Stripe checkout session and returns the payment URL
    Task<string> CreateCheckoutSessionAsync(Order order);
}
