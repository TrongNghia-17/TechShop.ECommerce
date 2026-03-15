namespace TechShop.ECommerce.Identity.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}
