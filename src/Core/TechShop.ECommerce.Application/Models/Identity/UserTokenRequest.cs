namespace TechShop.ECommerce.Application.Models.Identity;

public record UserTokenRequest(
    Guid UserId,
    string Email,
    IList<string> Roles);