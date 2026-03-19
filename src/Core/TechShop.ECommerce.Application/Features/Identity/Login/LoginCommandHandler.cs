using Microsoft.Extensions.Options;
using TechShop.ECommerce.Application.Common.Errors;
using TechShop.ECommerce.Application.Common.Results;
using TechShop.ECommerce.Application.Contracts.Authentication;
using TechShop.ECommerce.Application.Contracts.Identity;

namespace TechShop.ECommerce.Application.Features.Identity.Login;

public sealed class LoginCommandHandler(
    IIdentityService identityService,
    IJwtTokenGenerator tokenGenerator,
    IOptions<JwtOptions> jwtOptions,
    TimeProvider timeProvider)
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<Result<LoginResponse>> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var session = await identityService.LoginAsync(
            command.Email,
            command.Password,
            cancellationToken);

        if (session is null)
            return IdentityErrors.InvalidCredentials;

        var accessToken = await tokenGenerator.GenerateTokenAsync(
            new UserTokenRequest(
                session.UserId,
                session.Email,
                session.Roles.ToList()));

        return new LoginResponse(
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