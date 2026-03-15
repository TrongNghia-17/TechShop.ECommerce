namespace TechShop.ECommerce.Application.Common.Errors;

public static class IdentityErrors
{
    public static Error Unauthorized =>
        Error.Unauthorized(
            "Identity.Unauthorized",
            "User is not authorized.");

    public static Error EmailNotFound =>
        Error.Unauthorized(
            "Identity.EmailNotFound",
            "Current user email was not found.");

    public static Error InvalidCredentials =>
        Error.Unauthorized(
            "Identity.InvalidCredentials",
            "Invalid email or password.");

    public static Error MissingRefreshToken =>
        Error.Validation(
            "Identity.MissingRefreshToken",
            "Refresh token is required.");

    public static Error InvalidRefreshToken =>
        Error.Unauthorized(
            "Identity.InvalidRefreshToken",
            "Refresh token is invalid or inactive.");

    public static Error RegisterFailed(string message) =>
        Error.Validation(
            "Identity.RegisterFailed",
            message);
}
