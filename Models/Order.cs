using System;
using System.Collections.Generic;

// Represents a customer order, including payment and items.
namespace Twinstaranimation_backend.API.Models
{
    public class Order
    {
        public int Id { get; set; }

        // ID of the user who placed the order and navigation property to the user
        public required string UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        // Total price of the order and order status (e.g. Pending, Paid, Cancelled)
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";

        // Stripe session ID for payment tracking
        public string? StripeSessionId { get; set; }

        // Timestamp when order was created
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Item included in the order, with product details, quantity, and price at time of purchase
        public List<OrderItem> Items { get; set; } = new();
    }
}
