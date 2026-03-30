using Microsoft.Extensions.Options;
using TechShop.ECommerce.Application.Common.Errors;
using TechShop.ECommerce.Application.Common.Results;
using TechShop.ECommerce.Application.Contracts.Authentication;
using TechShop.ECommerce.Application.Contracts.Identity;

namespace TechShop.ECommerce.Application.Features.Identity.RefreshToken;

public sealed class RefreshTokenCommandHandler(
    IIdentityService identityService,
    IJwtTokenGenerator tokenGenerator,
    IOptions<JwtOptions> jwtOptions,
    TimeProvider timeProvider)
    : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<Result<RefreshTokenResponse>> Handle(
        RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.RefreshToken))
            return IdentityErrors.MissingRefreshToken;

        var session = await identityService.RefreshTokenAsync(
            command.RefreshToken,
            cancellationToken);

        if (session is null)
            return IdentityErrors.InvalidRefreshToken;

        var accessToken = await tokenGenerator.GenerateTokenAsync(
            new UserTokenRequest(
                session.UserId,
                session.Email,
                session.Roles.ToList()));

        return new RefreshTokenResponse(
            session.UserId,
            session.Email,
            session.UserName,
            accessToken,
            timeProvider.GetUtcNow().AddMinutes(_jwtOptions.DurationInMinutes),
            session.RefreshTokenExpiresAtUtc)
        {
            RefreshToken = session.RefreshToken
        };
    }
}