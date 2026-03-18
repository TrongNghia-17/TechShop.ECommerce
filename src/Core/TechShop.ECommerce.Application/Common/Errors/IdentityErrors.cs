namespace TechShop.ECommerce.Application.Common.Errors;

public static class IdentityErrors
{
    public static Domain.Errors.DomainErrors Unauthorized =>
        Domain.Errors.DomainErrors.Unauthorized(
            "Identity.Unauthorized",
            "User is not authorized.");

    public static Domain.Errors.DomainErrors EmailNotFound =>
        Domain.Errors.DomainErrors.Unauthorized(
            "Identity.EmailNotFound",
            "Current user email was not found.");

    public static Domain.Errors.DomainErrors InvalidCredentials =>
        Domain.Errors.DomainErrors.Unauthorized(
            "Identity.InvalidCredentials",
            "Invalid email or password.");

    public static Domain.Errors.DomainErrors MissingRefreshToken =>
        Domain.Errors.DomainErrors.Validation(
            "Identity.MissingRefreshToken",
            "Refresh token is required.");

    public static Domain.Errors.DomainErrors InvalidRefreshToken =>
        Domain.Errors.DomainErrors.Unauthorized(
            "Identity.InvalidRefreshToken",
            "Refresh token is invalid or inactive.");

    public static Domain.Errors.DomainErrors RegisterFailed(string message) =>
        Domain.Errors.DomainErrors.Validation(
            "Identity.RegisterFailed",
            message);
}
