namespace TechShop.ECommerce.Application.Features.Identity.Commands.RevokeRefreshToken;

public sealed record RevokeRefreshTokenCommand(string RefreshToken)
    : IRequest<Result>;