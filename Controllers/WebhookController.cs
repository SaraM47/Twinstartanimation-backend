using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Twinstaranimation_backend.API.Data;

// This controller handles incoming webhook events from Stripe.
// It is used to update order status after a successful payment.

[ApiController]
[Route("api/webhook")]
public class WebhookController : ControllerBase
{
    // Configuration for accessing Stripe webhook secret
    private readonly IConfiguration _config;

    // Database context for updating orders
    private readonly ApplicationDbContext _context;

    public WebhookController(IConfiguration config, ApplicationDbContext context)
    {
        _config = config;
        _context = context;
    }

    // Handles stripe webhook
    [HttpPost]
    public async Task<IActionResult> Handle()
    {
        // Read raw request body (Stripe sends JSON payload)
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            // Verify webhook signature to ensure request is from Stripe
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                _config["Stripe:WebhookSecret"]
            );

            // Chechout completed event indicates successful payment
            if (stripeEvent.Type == "checkout.session.completed")
            {
                // Extract session object
                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                // Null checks and validation for session and metadata
                if (session == null)
                    return BadRequest("Session is null");

                // Ensure metadata contains orderId
                if (session.Metadata == null || !session.Metadata.ContainsKey("orderId"))
                    return BadRequest("Missing orderId in metadata");

                // Convert orderId from string to int
                if (!int.TryParse(session.Metadata["orderId"], out var orderId))
                    return BadRequest("Invalid orderId format");

                // Find order in database
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                    return NotFound($"Order {orderId} not found");

                // Update order status to "Paid"
                order.Status = "Paid";

                await _context.SaveChangesAsync();
            }

            return Ok();
        }
        catch (StripeException e)
        {
            // Error related to Stripe validation
            return BadRequest($"Stripe error: {e.Message}");
        }
        catch (Exception e)
        {
            // General error handling
            return BadRequest($"Webhook error: {e.Message}");
        }
    }
}
