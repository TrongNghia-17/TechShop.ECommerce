namespace TechShop.ECommerce.Application.Contracts.Identity;

public sealed record IdentitySession(
    Guid UserId,
    string Email,
    string UserName,
    IReadOnlyCollection<string> Roles,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAtUtc);
