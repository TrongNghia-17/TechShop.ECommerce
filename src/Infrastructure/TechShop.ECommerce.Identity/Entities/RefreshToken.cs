namespace TechShop.ECommerce.Identity.Entities;

public sealed class RefreshToken
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string TokenHash { get; private set; } = default!;
    public DateTimeOffset ExpiresAtUtc { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }
    public DateTimeOffset? RevokedAtUtc { get; private set; }
    public string? ReplacedByTokenHash { get; private set; }
    public Guid UserId { get; private set; }
    public ApplicationUser User { get; private set; } = default!;

    private RefreshToken()
    {
    }

    private RefreshToken(
        string tokenHash,
        Guid userId,
        DateTimeOffset createdAtUtc,
        DateTimeOffset expiresAtUtc)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User ID is required.", nameof(userId));
        }

        TokenHash = NormalizeRequired(tokenHash, nameof(tokenHash));
        UserId = userId;
        CreatedAtUtc = createdAtUtc;
        ExpiresAtUtc = expiresAtUtc;
        Id = Guid.NewGuid();
    }

    public static RefreshToken Create(
        string tokenHash,
        Guid userId,
        DateTimeOffset createdAtUtc,
        DateTimeOffset expiresAtUtc)
    {
        return new RefreshToken(tokenHash, userId, createdAtUtc, expiresAtUtc);
    }

    public bool IsExpired(DateTimeOffset now) => now >= ExpiresAtUtc;

    public bool IsActive(DateTimeOffset now) => RevokedAtUtc is null && !IsExpired(now);

    public void Revoke(
        DateTimeOffset revokedAtUtc,
        string? replacedByTokenHash = null)
    {
        if (RevokedAtUtc is not null)
        {
            return;
        }

        RevokedAtUtc = revokedAtUtc;
        ReplacedByTokenHash = NormalizeOptional(replacedByTokenHash);
    }

    private static string NormalizeRequired(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value is required.", paramName);
        }

        return value.Trim();
    }

    private static string? NormalizeOptional(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim();
    }
}