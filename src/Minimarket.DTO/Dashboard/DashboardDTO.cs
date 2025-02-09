namespace Minimarket.DTO.Dashboard;

public class DashboardDTO
{
    public int? TotalSales { get; set; }

    public string? TotalRevenue { get; set; }

    public int? TotalProducts { get; set; }

    public List<SaleWeekDTO>? LastWeekSales { get; set; }
}