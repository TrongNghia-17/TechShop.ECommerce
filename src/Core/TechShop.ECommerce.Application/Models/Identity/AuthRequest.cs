namespace TechShop.ECommerce.Application.Models.Identity;

public class AuthRequest
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}
