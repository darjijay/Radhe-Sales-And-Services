using RadheSalesAndServices.Web.Models;

namespace RadheSalesAndServices.Web.Repositories;

public interface IStockRepository
{
    IEnumerable<Category> GetCategories();
    Category? GetCategoryById(int id);
    Category AddCategory(Category category);
    void UpdateCategory(Category category);
    void DeleteCategory(int id);

    IEnumerable<Product> GetProducts();
    Product? GetProductById(int id);
    Product AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(int id);

    IEnumerable<Customer> GetCustomers();
    Customer? GetCustomerById(int id);
    Customer AddCustomer(Customer customer);
    void UpdateCustomer(Customer customer);
    void DeleteCustomer(int id);

    IEnumerable<Sale> GetSales();
    Sale? GetSaleById(int id);
    Sale AddSale(Sale sale);
}
