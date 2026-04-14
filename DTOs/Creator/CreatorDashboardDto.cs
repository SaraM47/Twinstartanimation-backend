// This DTO is used to return dashboard data for a creator with statistics about the creator's performance

public class CreatorDashboardDto
{
    // Total revenue earned by the creator (money from sales)
    public decimal TotalRevenue { get; set; }

    // Total number of items sold
    public int TotalSales { get; set; }

    // Total number of orders placed
    public int TotalOrders { get; set; }

    // Total number of products created by the creator
    public int TotalProducts { get; set; }

    // Total number of series created by the creator
    public int TotalSeries { get; set; }

    // Total number of chapters across all series created by the creator
    public int TotalChapters { get; set; }
}
