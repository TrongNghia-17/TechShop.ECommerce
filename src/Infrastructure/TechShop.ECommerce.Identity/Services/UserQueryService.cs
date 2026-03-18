namespace TechShop.ECommerce.Identity.Services;

public sealed class UserQueryService(UserManager<ApplicationUser> userManager) : IUserQueryService
{
    public async Task<Customer> GetCustomer(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            throw new KeyNotFoundException($"User with id {userId} was not found.");
        }

        return MapToCustomer(user);
    }

    public async Task<List<Customer>> GetCustomers()
    {
        var users = await userManager.GetUsersInRoleAsync(Roles.Customer);

        return users
           .Select(MapToCustomer)
           .ToList();
    }

    private static Customer MapToCustomer(ApplicationUser user)
    {
        return new Customer(
            Id: user.Id,
            Email: user.Email ?? string.Empty,
            Firstname: user.FirstName,
            Lastname: user.LastName);
    }
}