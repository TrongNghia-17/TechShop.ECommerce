namespace TechShop.ECommerce.Application.Features.Identity.Commands.Login;

public record LoginResponse(
    Guid Id,
    string UserName,
    string Email,
    string Token
);
