using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RadheSalesAndServices.Web.ViewModels;

public class SaleCreateViewModel
{
    [Display(Name = "Customer")]
    [Required]
    public int CustomerId { get; set; }

    public List<SaleLineViewModel> Items { get; set; } = new();

    public IEnumerable<SelectListItem> Customers { get; set; } = Enumerable.Empty<SelectListItem>();
    public IEnumerable<SelectListItem> Products { get; set; } = Enumerable.Empty<SelectListItem>();
}

public class SaleLineViewModel
{
    [Display(Name = "Product")]
    [Required]
    public int ProductId { get; set; }

    [Range(1, 1000)]
    public int Quantity { get; set; } = 1;
}
