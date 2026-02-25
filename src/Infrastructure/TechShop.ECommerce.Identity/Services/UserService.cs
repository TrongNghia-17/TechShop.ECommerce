namespace TechShop.ECommerce.Identity.Services;

public class UserService(
    UserManager<ApplicationUser> userManager,
    IHttpContextAccessor contextAccessor)
    : IUserService
{
    public Guid UserId
    {
        get
        {
            var id = contextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(id, out var guid) ? guid : Guid.Empty;
        }
    }

    public async Task<Customer> GetCustomer(Guid userId)
    {
        var employee = await userManager.FindByIdAsync(userId.ToString());

        return employee == null
            ? throw new KeyNotFoundException($"User with id {userId} not found.")
            : new Customer
            {
                Email = employee.Email ?? string.Empty,
                Id = employee.Id,
                Firstname = employee.FirstName,
                Lastname = employee.LastName
            };
    }

    public async Task<List<Customer>> GetCustomers()
    {
        var employees = await userManager.GetUsersInRoleAsync("Employee");
        return [.. employees.Select(q => new Customer
        {
            Id = q.Id,
            Email = q.Email ?? string.Empty,
            Firstname = q.FirstName,
            Lastname = q.LastName
        })];
    }
}