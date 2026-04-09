// This class is used to store Stripe configuration settings.
public class StripeSettings
{
    // This property holds the Stripe Secret API key.
    // It is used on the backend to authenticate requests to Stripe.
    public string SecretKey { get; set; } = string.Empty;
}
