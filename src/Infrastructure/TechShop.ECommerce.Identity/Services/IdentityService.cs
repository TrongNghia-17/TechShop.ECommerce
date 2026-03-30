using System.Security.Cryptography;
using TechShop.ECommerce.Identity.Context;
using TechShop.ECommerce.Identity.Entities;

namespace TechShop.ECommerce.Identity.Services;

public sealed class IdentityService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    TechShopIdentityDbContext dbContext,
    IOptions<JwtOptions> jwtOptions,
    TimeProvider timeProvider) : IIdentityService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<IdentitySession?> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users
            .SingleOrDefaultAsync(x => x.Email == email, cancellationToken);

        if (user is null)
        {
            return null;
        }

        var signInResult = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);

        if (!signInResult.Succeeded)
        {
            return null;
        }

        return await CreateIdentitySessionAsync(user, cancellationToken);
    }

    public async Task<IdentitySession?> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return null;
        }

        var now = timeProvider.GetUtcNow();
        var refreshTokenHash = HashRefreshToken(refreshToken);

        var storedRefreshToken = await dbContext.RefreshTokens
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.TokenHash == refreshTokenHash, cancellationToken);

        if (storedRefreshToken is null || !storedRefreshToken.IsActive(now))
        {
            return null;
        }

        var user = storedRefreshToken.User;

        var newRefreshTokenValue = GenerateRefreshTokenValue();
        var newRefreshTokenHash = HashRefreshToken(newRefreshTokenValue);

        storedRefreshToken.Revoke(now, newRefreshTokenHash);

        var replacementRefreshToken = CreateRefreshToken(
            userId: user.Id,
            tokenHash: newRefreshTokenHash,
            createdAtUtc: now);

        await dbContext.RefreshTokens.AddAsync(replacementRefreshToken, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var roles = await userManager.GetRolesAsync(user);

        return new IdentitySession(
            user.Id,
            user.Email ?? string.Empty,
            user.UserName ?? string.Empty,
            roles.ToList(),
            newRefreshTokenValue,
            replacementRefreshToken.ExpiresAtUtc);
    }

    public async Task<bool> RevokeRefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            return false;
        }

        var now = timeProvider.GetUtcNow();
        var refreshTokenHash = HashRefreshToken(refreshToken);

        var storedRefreshToken = await dbContext.RefreshTokens
            .SingleOrDefaultAsync(x => x.TokenHash == refreshTokenHash, cancellationToken);

        if (storedRefreshToken is null || !storedRefreshToken.IsActive(now))
        {
            return false;
        }

        storedRefreshToken.Revoke(now);

        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<(bool Success, Guid UserId, string Errors)> RegisterAsync(
        string email,
        string userName,
        string firstName,
        string lastName,
        string password)
    {
        var user = new ApplicationUser
        {
            Email = email,
            UserName = userName,
            EmailConfirmed = true
        };

        var createUserResult = await userManager.CreateAsync(user, password);

        if (!createUserResult.Succeeded)
        {
            return (
                false,
                Guid.Empty,
                string.Join(Environment.NewLine, createUserResult.Errors.Select(x => x.Description)));
        }

        var addToRoleResult = await userManager.AddToRoleAsync(user, Roles.Customer);

        if (!addToRoleResult.Succeeded)
        {
            await userManager.DeleteAsync(user);

            return (
                false,
                Guid.Empty,
                string.Join(Environment.NewLine, addToRoleResult.Errors.Select(x => x.Description)));
        }

        return (true, user.Id, string.Empty);
    }

    private async Task<IdentitySession> CreateIdentitySessionAsync(
        ApplicationUser user,
        CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow();
        var roles = await userManager.GetRolesAsync(user);

        var refreshTokenValue = GenerateRefreshTokenValue();
        var refreshTokenHash = HashRefreshToken(refreshTokenValue);

        var refreshToken = CreateRefreshToken(
            userId: user.Id,
            tokenHash: refreshTokenHash,
            createdAtUtc: now);

        await dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new IdentitySession(
            user.Id,
            user.Email ?? string.Empty,
            user.UserName ?? string.Empty,
            roles.ToList(),
            refreshTokenValue,
            refreshToken.ExpiresAtUtc);
    }

    private RefreshToken CreateRefreshToken(
        Guid userId,
        string tokenHash,
        DateTimeOffset createdAtUtc)
    {
        return RefreshToken.Create(
            tokenHash,
            userId,
            createdAtUtc,
            createdAtUtc.AddDays(_jwtOptions.RefreshTokenLifetimeInDays));
    }

    private static string GenerateRefreshTokenValue()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Base64UrlEncoder.Encode(bytes);
    }

    private static string HashRefreshToken(string refreshToken)
    {
        var bytes = Encoding.UTF8.GetBytes(refreshToken);
        var hash = SHA256.HashData(bytes);

        return Convert.ToHexString(hash);
    }
}