using TechShop.ECommerce.Application.Common.Errors;
using TechShop.ECommerce.Application.Common.Results;
using TechShop.ECommerce.Application.Contracts.Identity;

namespace TechShop.ECommerce.Application.Features.Identity.RevokeRefreshToken;

public sealed class RevokeRefreshTokenCommandHandler(
    IIdentityService identityService)
    : IRequestHandler<RevokeRefreshTokenCommand, Result>
{
    public async Task<Result> Handle(
        RevokeRefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.RefreshToken))
            return IdentityErrors.MissingRefreshToken;

        var revoked = await identityService.RevokeRefreshTokenAsync(
            command.RefreshToken,
            cancellationToken);

        return revoked
            ? Result.Success()
            : IdentityErrors.InvalidRefreshToken;
    }
}