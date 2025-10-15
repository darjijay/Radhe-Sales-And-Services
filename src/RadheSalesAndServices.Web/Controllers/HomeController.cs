using Microsoft.AspNetCore.Mvc;
using RadheSalesAndServices.Web.Models;
using RadheSalesAndServices.Web.Repositories;
using RadheSalesAndServices.Web.ViewModels;
using System.Diagnostics;

namespace RadheSalesAndServices.Web.Controllers;

public class HomeController : Controller
{
    private readonly IStockRepository _repository;

    public HomeController(IStockRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        var products = _repository.GetProducts().ToList();
        var sales = _repository.GetSales().ToList();
        var currentMonth = DateTime.UtcNow.Month;
        var currentYear = DateTime.UtcNow.Year;

        var viewModel = new DashboardViewModel
        {
            TotalProducts = products.Count,
            LowStockProducts = products.Count(p => p.StockQuantity <= p.ReorderLevel),
            TotalCategories = _repository.GetCategories().Count(),
            TotalCustomers = _repository.GetCustomers().Count(),
            MonthlySales = sales
                .Where(s => s.SaleDate.Month == currentMonth && s.SaleDate.Year == currentYear)
                .Sum(s => s.TotalAmount),
            TopProducts = products
                .Where(p => p.StockQuantity <= p.ReorderLevel)
                .OrderBy(p => p.StockQuantity)
                .Take(5),
            RecentSales = sales
                .OrderByDescending(s => s.SaleDate)
                .Take(5)
        };

        return View(viewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
