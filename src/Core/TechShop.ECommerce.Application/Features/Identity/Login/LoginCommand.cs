using TechShop.ECommerce.Application.Common.Results;

namespace TechShop.ECommerce.Application.Features.Identity.Login;

public sealed record LoginCommand(
    string Email,
    string Password
) : IRequest<Result<LoginResponse>>;
