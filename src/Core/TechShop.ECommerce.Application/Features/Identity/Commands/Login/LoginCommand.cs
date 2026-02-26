namespace TechShop.ECommerce.Application.Features.Identity.Commands.Login;

public sealed record LoginCommand(
    string Email,
    string Password
) : IRequest<LoginResponse>;
