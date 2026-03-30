using TechShop.ECommerce.Application.Common.Results;

namespace TechShop.ECommerce.Application.Features.Identity.RevokeRefreshToken;

public sealed record RevokeRefreshTokenCommand(string RefreshToken)
    : IRequest<Result>;