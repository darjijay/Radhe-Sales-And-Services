using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RadheSalesAndServices.Web.Models;
using RadheSalesAndServices.Web.Repositories;
using RadheSalesAndServices.Web.ViewModels;

namespace RadheSalesAndServices.Web.Controllers;

public class SalesController : Controller
{
    private readonly IStockRepository _repository;

    public SalesController(IStockRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        var sales = _repository.GetSales();
        return View(sales);
    }

    public IActionResult Details(int id)
    {
        var sale = _repository.GetSaleById(id);
        if (sale is null)
        {
            return NotFound();
        }

        return View(sale);
    }

    public IActionResult Create()
    {
        var viewModel = new SaleCreateViewModel
        {
            Items = new List<SaleLineViewModel> { new() }
        };

        PopulateSelections(viewModel);
        EnsureAtLeastOneRow(viewModel);
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(SaleCreateViewModel viewModel)
    {
        viewModel.Items ??= new List<SaleLineViewModel>();
        viewModel.Items = viewModel.Items
            .Where(item => item != null)
            .ToList();

        if (!viewModel.Items.Any())
        {
            ModelState.AddModelError(string.Empty, "Please add at least one product to the sale.");
        }

        if (!ModelState.IsValid)
        {
            PopulateSelections(viewModel);
            EnsureAtLeastOneRow(viewModel);
            return View(viewModel);
        }

        var sale = new Sale
        {
            CustomerId = viewModel.CustomerId,
            Items = viewModel.Items
                .Where(i => i.ProductId > 0 && i.Quantity > 0)
                .Select(i => new SaleItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                })
                .ToList()
        };

        if (!sale.Items.Any())
        {
            ModelState.AddModelError(string.Empty, "Please add at least one product to the sale.");
            PopulateSelections(viewModel);
            EnsureAtLeastOneRow(viewModel);
            return View(viewModel);
        }

        try
        {
            _repository.AddSale(sale);
            TempData["Success"] = "Sale recorded successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            PopulateSelections(viewModel);
            EnsureAtLeastOneRow(viewModel);
            return View(viewModel);
        }
    }

    private void PopulateSelections(SaleCreateViewModel viewModel)
    {
        viewModel.Customers = _repository.GetCustomers()
            .Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            })
            .ToList();

        viewModel.Products = _repository.GetProducts()
            .Select(p => new SelectListItem
            {
                Text = $"{p.Name} (Stock: {p.StockQuantity})",
                Value = p.Id.ToString()
            })
            .ToList();
    }

    private static void EnsureAtLeastOneRow(SaleCreateViewModel viewModel)
    {
        if (!viewModel.Items.Any())
        {
            viewModel.Items.Add(new SaleLineViewModel());
        }
    }
}
