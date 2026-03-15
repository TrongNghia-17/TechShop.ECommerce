using Microsoft.Extensions.Options;
using TechShop.ECommerce.Application.Contracts.Identity;
using TechShop.ECommerce.Application.Features.Identity.Commands.RefreshToken;
using TechShop.ECommerce.Application.Features.Identity.Commands.RevokeRefreshToken;

namespace TechShop.ECommerce.Api.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("Auth");

        group.MapPost("/login",
            async (
                [FromBody] LoginCommand command,
                ISender sender,
                IOptions<JwtOptions> jwtOptions,
                HttpContext httpContext,
                CancellationToken token) =>
            {
                var result = await sender.Send(command, token);

                if (!result.IsSuccess)
                    return result.ToApiResult();

                httpContext.Response.AppendRefreshTokenCookie(
                    result.Value.RefreshToken,
                    jwtOptions.Value);

                return Results.Ok(Result<LoginResponse>.Success(result.Value));
            });

        group.MapPost("/refresh-token",
            async (
                ISender sender,
                IOptions<JwtOptions> jwtOptions,
                HttpContext httpContext,
                CancellationToken token) =>
            {
                var refreshToken =
                    httpContext.Request.Cookies[jwtOptions.Value.RefreshTokenCookieName];

                var result = await sender.Send(
                    new RefreshTokenCommand(refreshToken ?? string.Empty),
                    token);

                if (!result.IsSuccess)
                    return result.ToApiResult();

                httpContext.Response.AppendRefreshTokenCookie(
                    result.Value.RefreshToken,
                    jwtOptions.Value);

                return Results.Ok(Result<RefreshTokenResponse>.Success(result.Value));
            });

        group.MapPost("/revoke-refresh-token",
            async (
                [FromBody] RevokeRefreshTokenRequest request,
                ISender sender,
                IOptions<JwtOptions> jwtOptions,
                HttpContext httpContext,
                CancellationToken token) =>
            {
                var refreshToken =
                    httpContext.Request.Cookies[jwtOptions.Value.RefreshTokenCookieName]
                    ?? request.RefreshToken;

                var result = await sender.Send(
                    new RevokeRefreshTokenCommand(refreshToken ?? string.Empty),
                    token);

                if (!result.IsSuccess)
                    return result.ToApiResult();

                httpContext.Response.DeleteRefreshTokenCookie(jwtOptions.Value);

                return result.ToApiResult();
            });

        return group;
    }
}