using TechShop.ECommerce.Application.Contracts.Identity;

namespace TechShop.ECommerce.Api.Extensions;

public static class RefreshTokenCookieExtensions
{
    public static void AppendRefreshTokenCookie(
        this HttpResponse response,
        string refreshToken,
        JwtOptions jwtOptions)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(jwtOptions.RefreshTokenLifetimeInDays),
            Path = "/"
        };

        response.Cookies.Append(
            jwtOptions.RefreshTokenCookieName,
            refreshToken,
            options);
    }

    public static void DeleteRefreshTokenCookie(
        this HttpResponse response,
        JwtOptions jwtOptions)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            IsEssential = true,
            SameSite = SameSiteMode.Lax,
            Path = "/"
        };

        response.Cookies.Delete(jwtOptions.RefreshTokenCookieName, options);
    }
}