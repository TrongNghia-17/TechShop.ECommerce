namespace TechShop.ECommerce.Application.Contracts.Identity;

public interface IIdentityService
{
    Task<IdentitySession?> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);

    Task<IdentitySession?> RefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);

    Task<bool> RevokeRefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);

    Task<(bool Success, Guid UserId, string Errors)>
        RegisterAsync(
            string email,
            string userName,
            string firstName,
            string lastName,
            string password);
}
