namespace TechShop.ECommerce.Application.Contracts.Identity;

public interface IIdentityService
{
    Task<(bool Success, Guid UserId, string Email, string UserName, IList<string> Roles)>
        LoginAsync(string email, string password);

    Task<(bool Success, Guid UserId, string Errors)>
        RegisterAsync(
            string email,
            string userName,
            string firstName,
            string lastName,
            string password);
}
