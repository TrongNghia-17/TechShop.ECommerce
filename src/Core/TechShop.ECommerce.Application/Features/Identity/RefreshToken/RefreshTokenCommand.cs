using TechShop.ECommerce.Application.Common.Results;

namespace TechShop.ECommerce.Application.Features.Identity.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken)
    : IRequest<Result<RefreshTokenResponse>>;
