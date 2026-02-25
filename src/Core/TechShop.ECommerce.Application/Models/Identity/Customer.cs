namespace TechShop.ECommerce.Application.Models.Identity;

public class Customer
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public string Firstname { get; set; } = default!;
    public string Lastname { get; set; } = default!;
}
