using RadheSalesAndServices.Web.Models;

namespace RadheSalesAndServices.Web.ViewModels;

public class DashboardViewModel
{
    public int TotalProducts { get; set; }
    public int LowStockProducts { get; set; }
    public int TotalCategories { get; set; }
    public int TotalCustomers { get; set; }
    public decimal MonthlySales { get; set; }
    public IEnumerable<Product> TopProducts { get; set; } = Enumerable.Empty<Product>();
    public IEnumerable<Sale> RecentSales { get; set; } = Enumerable.Empty<Sale>();
}
