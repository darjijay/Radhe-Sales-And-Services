using Microsoft.Extensions.DependencyInjection;
using RadheSalesAndServices.Web.Models;
using RadheSalesAndServices.Web.Repositories;

namespace RadheSalesAndServices.Web.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider services)
    {
        var repository = services.GetRequiredService<IStockRepository>();
        if (repository.GetCategories().Any())
        {
            return;
        }

        var categories = new List<Category>
        {
            new() { Name = "LED Lights", Description = "LED bulbs, tubes, and accessories" },
            new() { Name = "Switches & Sockets", Description = "Modular switches, sockets, and plates" },
            new() { Name = "Wires & Cables", Description = "Copper wires, cables, and conduits" }
        };

        foreach (var category in categories)
        {
            repository.AddCategory(category);
        }

        var products = new List<Product>
        {
            new() { Name = "12W LED Bulb", Sku = "LED-12W", UnitPrice = 120, StockQuantity = 150, ReorderLevel = 25, CategoryId = 1 },
            new() { Name = "18W LED Tube Light", Sku = "TL-18W", UnitPrice = 340, StockQuantity = 90, ReorderLevel = 15, CategoryId = 1 },
            new() { Name = "Modular Switch 6A", Sku = "SW-6A", UnitPrice = 75, StockQuantity = 220, ReorderLevel = 40, CategoryId = 2 },
            new() { Name = "3-Core Copper Cable 1.5mm", Sku = "CB-3C-15", UnitPrice = 980, StockQuantity = 60, ReorderLevel = 10, CategoryId = 3 }
        };

        foreach (var product in products)
        {
            repository.AddProduct(product);
        }

        var customers = new List<Customer>
        {
            new() { Name = "Akash Electricals", Email = "contact@akashelectricals.in", Phone = "+91 98765 43210", Address = "Industrial Area, Surat" },
            new() { Name = "Bright Homes", Email = "sales@brighthomes.in", Phone = "+91 91234 56780", Address = "Ring Road, Surat" }
        };

        foreach (var customer in customers)
        {
            repository.AddCustomer(customer);
        }

        repository.AddSale(new Sale
        {
            CustomerId = 1,
            SaleDate = DateTime.UtcNow.AddDays(-2),
            Items = new List<SaleItem>
            {
                new() { ProductId = 1, Quantity = 10 },
                new() { ProductId = 3, Quantity = 15 }
            }
        });

        repository.AddSale(new Sale
        {
            CustomerId = 2,
            SaleDate = DateTime.UtcNow.AddDays(-1),
            Items = new List<SaleItem>
            {
                new() { ProductId = 2, Quantity = 5 }
            }
        });
    }
}
