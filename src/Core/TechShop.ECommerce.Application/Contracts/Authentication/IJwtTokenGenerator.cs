using TechShop.ECommerce.Application.Contracts.Identity;

namespace TechShop.ECommerce.Application.Contracts.Authentication;

public interface IJwtTokenGenerator
{
    Task<string> GenerateTokenAsync(UserTokenRequest request);
}
