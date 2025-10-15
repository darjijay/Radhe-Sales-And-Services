using RadheSalesAndServices.Web.Models;

namespace RadheSalesAndServices.Web.Repositories;

public class InMemoryStockRepository : IStockRepository
{
    private readonly List<Category> _categories = new();
    private readonly List<Product> _products = new();
    private readonly List<Customer> _customers = new();
    private readonly List<Sale> _sales = new();
    private int _categoryId = 1;
    private int _productId = 1;
    private int _customerId = 1;
    private int _saleId = 1;
    private readonly object _lock = new();

    public IEnumerable<Category> GetCategories()
    {
        lock (_lock)
        {
            return _categories
                .Select(c => new Category
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .OrderBy(c => c.Name)
                .ToList();
        }
    }

    public Category? GetCategoryById(int id)
    {
        lock (_lock)
        {
            return _categories.FirstOrDefault(c => c.Id == id)?.Clone();
        }
    }

    public Category AddCategory(Category category)
    {
        lock (_lock)
        {
            category.Id = _categoryId++;
            _categories.Add(category.Clone());
            return category;
        }
    }

    public void UpdateCategory(Category category)
    {
        lock (_lock)
        {
            var existing = _categories.FirstOrDefault(c => c.Id == category.Id);
            if (existing is null)
            {
                return;
            }

            existing.Name = category.Name;
            existing.Description = category.Description;
        }
    }

    public void DeleteCategory(int id)
    {
        lock (_lock)
        {
            if (_products.Any(p => p.CategoryId == id))
            {
                throw new InvalidOperationException("Cannot delete a category that still has products.");
            }

            _categories.RemoveAll(c => c.Id == id);
        }
    }

    public IEnumerable<Product> GetProducts()
    {
        lock (_lock)
        {
            return _products
                .Select(CloneProduct)
                .OrderBy(p => p.Name)
                .ToList();
        }
    }

    public Product? GetProductById(int id)
    {
        lock (_lock)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return product is null ? null : CloneProduct(product);
        }
    }

    public Product AddProduct(Product product)
    {
        lock (_lock)
        {
            product.Id = _productId++;
            var category = _categories.FirstOrDefault(c => c.Id == product.CategoryId);
            if (category is not null)
            {
                product.Category = category.Clone();
            }

            _products.Add(CloneProduct(product));
            return product;
        }
    }

    public void UpdateProduct(Product product)
    {
        lock (_lock)
        {
            var existing = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existing is null)
            {
                return;
            }

            existing.Name = product.Name;
            existing.Sku = product.Sku;
            existing.UnitPrice = product.UnitPrice;
            existing.StockQuantity = product.StockQuantity;
            existing.ReorderLevel = product.ReorderLevel;
            existing.CategoryId = product.CategoryId;

            existing.Category = _categories.FirstOrDefault(c => c.Id == product.CategoryId)?.Clone();
        }
    }

    public void DeleteProduct(int id)
    {
        lock (_lock)
        {
            if (_sales.Any(s => s.Items.Any(i => i.ProductId == id)))
            {
                throw new InvalidOperationException("Cannot delete a product that has been sold.");
            }

            _products.RemoveAll(p => p.Id == id);
        }
    }

    public IEnumerable<Customer> GetCustomers()
    {
        lock (_lock)
        {
            return _customers
                .Select(c => c.Clone())
                .OrderBy(c => c.Name)
                .ToList();
        }
    }

    public Customer? GetCustomerById(int id)
    {
        lock (_lock)
        {
            return _customers.FirstOrDefault(c => c.Id == id)?.Clone();
        }
    }

    public Customer AddCustomer(Customer customer)
    {
        lock (_lock)
        {
            customer.Id = _customerId++;
            _customers.Add(customer.Clone());
            return customer;
        }
    }

    public void UpdateCustomer(Customer customer)
    {
        lock (_lock)
        {
            var existing = _customers.FirstOrDefault(c => c.Id == customer.Id);
            if (existing is null)
            {
                return;
            }

            existing.Name = customer.Name;
            existing.Email = customer.Email;
            existing.Phone = customer.Phone;
            existing.Address = customer.Address;
        }
    }

    public void DeleteCustomer(int id)
    {
        lock (_lock)
        {
            if (_sales.Any(s => s.CustomerId == id))
            {
                throw new InvalidOperationException("Cannot delete a customer with sales history.");
            }

            _customers.RemoveAll(c => c.Id == id);
        }
    }

    public IEnumerable<Sale> GetSales()
    {
        lock (_lock)
        {
            return _sales
                .Select(CloneSale)
                .OrderByDescending(s => s.SaleDate)
                .ToList();
        }
    }

    public Sale? GetSaleById(int id)
    {
        lock (_lock)
        {
            var sale = _sales.FirstOrDefault(s => s.Id == id);
            return sale is null ? null : CloneSale(sale);
        }
    }

    public Sale AddSale(Sale sale)
    {
        lock (_lock)
        {
            sale.Id = _saleId++;
            sale.SaleDate = sale.SaleDate == default ? DateTime.UtcNow : sale.SaleDate;

            foreach (var item in sale.Items)
            {
                var product = _products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product is null)
                {
                    throw new InvalidOperationException("Product not found.");
                }

                if (item.Quantity <= 0)
                {
                    throw new InvalidOperationException("Quantity must be greater than zero.");
                }

                if (product.StockQuantity < item.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for {product.Name}.");
                }

                product.StockQuantity -= item.Quantity;
                item.UnitPrice = product.UnitPrice;
                item.Product = product.Clone();
            }

            var customer = _customers.FirstOrDefault(c => c.Id == sale.CustomerId);
            sale.Customer = customer?.Clone();

            _sales.Add(CloneSale(sale));
            return sale;
        }
    }

    private Product CloneProduct(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Sku = product.Sku,
        UnitPrice = product.UnitPrice,
        StockQuantity = product.StockQuantity,
        ReorderLevel = product.ReorderLevel,
        CategoryId = product.CategoryId,
        Category = product.Category?.Clone()
    };

    private Sale CloneSale(Sale sale)
    {
        return new Sale
        {
            Id = sale.Id,
            SaleDate = sale.SaleDate,
            CustomerId = sale.CustomerId,
            Customer = sale.Customer?.Clone(),
            Items = sale.Items.Select(i => new SaleItem
            {
                ProductId = i.ProductId,
                Product = i.Product?.Clone(),
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };
    }
}

internal static class ModelCloner
{
    public static Category Clone(this Category category) => new()
    {
        Id = category.Id,
        Name = category.Name,
        Description = category.Description
    };

    public static Customer Clone(this Customer customer) => new()
    {
        Id = customer.Id,
        Name = customer.Name,
        Email = customer.Email,
        Phone = customer.Phone,
        Address = customer.Address
    };

    public static Product Clone(this Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Sku = product.Sku,
        UnitPrice = product.UnitPrice,
        StockQuantity = product.StockQuantity,
        ReorderLevel = product.ReorderLevel,
        CategoryId = product.CategoryId,
        Category = product.Category?.Clone()
    };
}
