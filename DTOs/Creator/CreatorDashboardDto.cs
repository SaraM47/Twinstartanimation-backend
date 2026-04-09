// This DTO is used to return dashboard data for a creator with statistics about the creator's performance

public class CreatorDashboardDto
{
    // Total revenue earned by the creator (money from sales)
    public decimal TotalRevenue { get; set; }

    // Total number of items sold
    public int TotalSales { get; set; }

    // Total number of orders placed
    public int TotalOrders { get; set; }
}
