using TechShop.ECommerce.Application.Common.Constants;

namespace TechShop.ECommerce.Identity.Services;

public class UserQueryService(
    UserManager<ApplicationUser> userManager)
    : IUserQueryService
{
    public async Task<Customer> GetCustomer(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user == null)
            throw new KeyNotFoundException(
                $"User with id {userId} not found.");

        return new Customer
        (
            Id: user.Id,
            Email: user.Email ?? string.Empty,
            Firstname: user.FirstName,
            Lastname: user.LastName
        );
    }

    public async Task<List<Customer>> GetCustomers()
    {
        var users = await userManager
            .GetUsersInRoleAsync(Roles.Customer);

        return users.Select(user => new Customer
        (
            Id: user.Id,
            Email: user.Email ?? string.Empty,
            Firstname: user.FirstName,
            Lastname: user.LastName
        )).ToList();
    }
}