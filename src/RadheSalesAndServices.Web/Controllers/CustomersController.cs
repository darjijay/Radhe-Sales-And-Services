using Microsoft.AspNetCore.Mvc;
using RadheSalesAndServices.Web.Models;
using RadheSalesAndServices.Web.Repositories;

namespace RadheSalesAndServices.Web.Controllers;

public class CustomersController : Controller
{
    private readonly IStockRepository _repository;

    public CustomersController(IStockRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        var customers = _repository.GetCustomers();
        return View(customers);
    }

    public IActionResult Create()
    {
        return View(new Customer());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Customer customer)
    {
        if (!ModelState.IsValid)
        {
            return View(customer);
        }

        _repository.AddCustomer(customer);
        TempData["Success"] = "Customer created successfully.";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var customer = _repository.GetCustomerById(id);
        if (customer is null)
        {
            return NotFound();
        }

        return View(customer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Customer customer)
    {
        if (id != customer.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(customer);
        }

        _repository.UpdateCustomer(customer);
        TempData["Success"] = "Customer updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int id)
    {
        var customer = _repository.GetCustomerById(id);
        if (customer is null)
        {
            return NotFound();
        }

        return View(customer);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            _repository.DeleteCustomer(id);
            TempData["Success"] = "Customer deleted successfully.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
