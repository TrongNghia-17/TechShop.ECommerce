namespace TechShop.ECommerce.Application.Common.Errors;

public static class IdentityErrors
{
    public static Error Unauthorized =>
        Error.Unauthorized(
            "Identity.Unauthorized",
            "User is not authorized.");

    public static Error EmailNotFound =>
        Error.Validation(
            "Identity.EmailNotFound",
            "Current user email was not found.");

    public static Error InvalidCredentials =>
        Error.Validation(
            "Identity.InvalidCredentials",
            "Invalid email or password.");

    public static Error RegisterFailed(string message) =>
        Error.Validation(
            "Identity.RegisterFailed",
            message);
}
