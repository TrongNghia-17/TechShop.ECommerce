namespace TechShop.ECommerce.Application.Contracts.Identity;

public record UserTokenRequest(
    Guid UserId,
    string Email,
    IList<string> Roles);