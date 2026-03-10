using TechShop.ECommerce.Application.Contracts.Identity;

namespace TechShop.ECommerce.Infrastructure.Identity;

public sealed class BackgroundCurrentUserService : ICurrentUserService
{
    public Guid UserId => Guid.Empty;

    public string? Email => null;
}
