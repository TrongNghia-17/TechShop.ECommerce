namespace TechShop.ECommerce.Identity.Entities;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public ICollection<RefreshToken> RefreshTokens { get; private set; } = [];
}