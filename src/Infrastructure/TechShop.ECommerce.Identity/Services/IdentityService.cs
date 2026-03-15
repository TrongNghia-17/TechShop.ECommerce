using System.Security.Cryptography;

namespace TechShop.ECommerce.Identity.Services;

public class IdentityService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    TechShopIdentityDbContext dbContext,
    IOptions<JwtOptions> jwtOptions,
    TimeProvider timeProvider)
    : IIdentityService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<IdentitySession?> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users
            .Include(x => x.RefreshTokens)
            .SingleOrDefaultAsync(x => x.Email == email, cancellationToken);

        if (user is null)
            return null;

        var result = await signInManager.CheckPasswordSignInAsync(user, password, false);

        if (!result.Succeeded)
            return null;

        var roles = await userManager.GetRolesAsync(user);
        var now = timeProvider.GetUtcNow();

        var refreshTokenValue = GenerateRefreshTokenValue();
        var refreshTokenHash = HashRefreshToken(refreshTokenValue);

        var refreshToken = RefreshToken.Create(
            refreshTokenHash,
            user.Id,
            now,
            now.AddDays(_jwtOptions.RefreshTokenLifetimeInDays));

        await dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new IdentitySession(
            user.Id,
            user.Email!,
            user.UserName!,
            roles.ToList(),
            refreshTokenValue,
            refreshToken.ExpiresAtUtc);
    }

    public async Task<IdentitySession?> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return null;

        var now = timeProvider.GetUtcNow();
        var refreshTokenHash = HashRefreshToken(refreshToken);

        var storedToken = await dbContext.RefreshTokens
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.TokenHash == refreshTokenHash, cancellationToken);

        if (storedToken is null)
            return null;

        if (!storedToken.IsActive(now))
            return null;

        var user = storedToken.User;
        var roles = await userManager.GetRolesAsync(user);

        var newRefreshTokenValue = GenerateRefreshTokenValue();
        var newRefreshTokenHash = HashRefreshToken(newRefreshTokenValue);

        storedToken.Revoke(now, newRefreshTokenHash);

        var replacementToken = RefreshToken.Create(
            newRefreshTokenHash,
            user.Id,
            now,
            now.AddDays(_jwtOptions.RefreshTokenLifetimeInDays));

        await dbContext.RefreshTokens.AddAsync(replacementToken, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new IdentitySession(
            user.Id,
            user.Email!,
            user.UserName!,
            roles.ToList(),
            newRefreshTokenValue,
            replacementToken.ExpiresAtUtc);
    }

    public async Task<bool> RevokeRefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return false;

        var now = timeProvider.GetUtcNow();
        var refreshTokenHash = HashRefreshToken(refreshToken);

        var storedToken = await dbContext.RefreshTokens
            .SingleOrDefaultAsync(x => x.TokenHash == refreshTokenHash, cancellationToken);

        if (storedToken is null)
            return false;

        if (!storedToken.IsActive(now))
            return false;

        storedToken.Revoke(now);

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
            FirstName = firstName,
            LastName = lastName,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = string.Join("\n", result.Errors.Select(x => x.Description));
            return (false, Guid.Empty, errors);
        }

        await userManager.AddToRoleAsync(user, Roles.Customer);

        return (true, user.Id, string.Empty);
    }

    private static string GenerateRefreshTokenValue()
    {
        Span<byte> bytes = stackalloc byte[32];
        RandomNumberGenerator.Fill(bytes);

        return Convert.ToBase64String(bytes);
    }

    private static string HashRefreshToken(string refreshToken)
    {
        var bytes = Encoding.UTF8.GetBytes(refreshToken);
        var hash = SHA256.HashData(bytes);

        return Convert.ToHexString(hash);
    }
}