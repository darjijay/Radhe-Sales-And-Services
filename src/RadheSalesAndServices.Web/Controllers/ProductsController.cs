using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RadheSalesAndServices.Web.Models;
using RadheSalesAndServices.Web.Repositories;

namespace RadheSalesAndServices.Web.Controllers;

public class ProductsController : Controller
{
    private readonly IStockRepository _repository;

    public ProductsController(IStockRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        var products = _repository.GetProducts();
        return View(products);
    }

    public IActionResult Create()
    {
        PopulateCategories();
        return View(new Product());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product product)
    {
        if (!ModelState.IsValid)
        {
            PopulateCategories();
            return View(product);
        }

        _repository.AddProduct(product);
        TempData["Success"] = "Product created successfully.";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var product = _repository.GetProductById(id);
        if (product is null)
        {
            return NotFound();
        }

        PopulateCategories();
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            PopulateCategories();
            return View(product);
        }

        _repository.UpdateProduct(product);
        TempData["Success"] = "Product updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        var product = _repository.GetProductById(id);
        if (product is null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            _repository.DeleteProduct(id);
            TempData["Success"] = "Product deleted successfully.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    private void PopulateCategories()
    {
        ViewBag.Categories = _repository.GetCategories()
            .Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            })
            .ToList();
    }
}
