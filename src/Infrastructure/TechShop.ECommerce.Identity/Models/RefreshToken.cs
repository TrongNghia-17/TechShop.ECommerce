namespace TechShop.ECommerce.Identity.Models;

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

    public bool IsExpired(DateTimeOffset now) => now >= ExpiresAtUtc;

    public bool IsActive(DateTimeOffset now) => RevokedAtUtc is null && !IsExpired(now);

    private RefreshToken()
    {
    }

    public static RefreshToken Create(
        string tokenHash,
        Guid userId,
        DateTimeOffset createdAtUtc,
        DateTimeOffset expiresAtUtc)
    {
        return new RefreshToken
        {
            TokenHash = tokenHash,
            UserId = userId,
            CreatedAtUtc = createdAtUtc,
            ExpiresAtUtc = expiresAtUtc
        };
    }

    public void Revoke(
        DateTimeOffset revokedAtUtc,
        string? replacedByTokenHash = null)
    {
        if (RevokedAtUtc is not null)
            return;

        RevokedAtUtc = revokedAtUtc;
        ReplacedByTokenHash = replacedByTokenHash;
    }
}