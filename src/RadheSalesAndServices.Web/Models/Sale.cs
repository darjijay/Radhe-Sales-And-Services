namespace RadheSalesAndServices.Web.Models;

public class Sale
{
    public int Id { get; set; }
    public DateTime SaleDate { get; set; }
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public List<SaleItem> Items { get; set; } = new();
    public decimal TotalAmount => Items.Sum(i => i.LineTotal);
}
