using Microsoft.Extensions.Options;
using TechShop.ECommerce.Api.Extensions.Http;
using TechShop.ECommerce.Api.Extensions.RateLimiting;
using TechShop.ECommerce.Application.Common.Results;
using TechShop.ECommerce.Application.Contracts.Identity;
using TechShop.ECommerce.Application.Features.Identity.Login;
using TechShop.ECommerce.Application.Features.Identity.RefreshToken;
using TechShop.ECommerce.Application.Features.Identity.RevokeRefreshToken;

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
            })
            .WithName("Auth_Login")
            .WithSummary("User login")
            .WithDescription("""
                Authenticates the user and returns a short-lived JWT access token.
                A refresh token is issued in a secure HttpOnly cookie.
                """)
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status429TooManyRequests)
            .RequireRateLimiting(RateLimitPolicies.AuthFixed);

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
            })
            .WithName("Auth_RefreshToken")
            .WithSummary("Refresh access token")
            .WithDescription("""
                Uses the refresh token stored in the secure HttpOnly cookie
                to issue a new JWT access token and rotate the refresh token.
                """)
            .Produces<RefreshTokenResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status429TooManyRequests)
            .RequireRateLimiting(RateLimitPolicies.AuthFixed);

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
            })
            .WithName("Auth_RevokeRefreshToken")
            .WithSummary("Revoke refresh token")
            .WithDescription("""
                Revokes the current refresh token and removes the refresh token cookie.
                """)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status429TooManyRequests)
            .RequireRateLimiting(RateLimitPolicies.AuthFixed);

        return group;
    }
}