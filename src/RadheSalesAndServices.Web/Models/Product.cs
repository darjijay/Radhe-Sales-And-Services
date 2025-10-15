using System.ComponentModel.DataAnnotations;

namespace RadheSalesAndServices.Web.Models;

public class Product
{
    public int Id { get; set; }
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [StringLength(50)]
    public string Sku { get; set; } = string.Empty;
    [Range(0, 1000000)]
    public decimal UnitPrice { get; set; }
    [Range(0, 1000000)]
    public int StockQuantity { get; set; }
    [Range(0, 1000000)]
    public int ReorderLevel { get; set; }
    [Display(Name = "Category")]
    [Required]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
