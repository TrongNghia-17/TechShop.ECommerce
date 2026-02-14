namespace TechShop.ECommerce.Application.Models.Identity;

public class AuthResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Token { get; set; } = default!;
}
