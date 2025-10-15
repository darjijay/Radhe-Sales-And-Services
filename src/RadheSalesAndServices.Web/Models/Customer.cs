using System.ComponentModel.DataAnnotations;

namespace RadheSalesAndServices.Web.Models;

public class Customer
{
    public int Id { get; set; }
    [Required]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;
    [Phone]
    [StringLength(30)]
    public string Phone { get; set; } = string.Empty;
    [StringLength(200)]
    public string? Address { get; set; }
}
