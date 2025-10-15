using Microsoft.AspNetCore.Mvc;
using RadheSalesAndServices.Web.Models;
using RadheSalesAndServices.Web.Repositories;

namespace RadheSalesAndServices.Web.Controllers;

public class CategoriesController : Controller
{
    private readonly IStockRepository _repository;

    public CategoriesController(IStockRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        var categories = _repository.GetCategories();
        return View(categories);
    }

    public IActionResult Create()
    {
        return View(new Category());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }

        _repository.AddCategory(category);
        TempData["Success"] = "Category created successfully.";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var category = _repository.GetCategoryById(id);
        if (category is null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Category category)
    {
        if (id != category.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(category);
        }

        _repository.UpdateCategory(category);
        TempData["Success"] = "Category updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        var category = _repository.GetCategoryById(id);
        if (category is null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            _repository.DeleteCategory(id);
            TempData["Success"] = "Category deleted successfully.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
