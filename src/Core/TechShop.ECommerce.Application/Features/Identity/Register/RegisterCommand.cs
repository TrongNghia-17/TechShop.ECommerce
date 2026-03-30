using TechShop.ECommerce.Application.Common.Results;

namespace TechShop.ECommerce.Application.Features.Identity.Register;

public sealed record RegisterCommand(
    string Email,
    string UserName,
    string FirstName,
    string LastName,
    string Password
) : IRequest<Result<RegisterResponse>>;