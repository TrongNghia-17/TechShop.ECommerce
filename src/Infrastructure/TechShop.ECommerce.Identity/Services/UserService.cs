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

    public async Task<Employee> GetEmployee(Guid userId)
    {
        var employee = await userManager.FindByIdAsync(userId.ToString());

        return employee == null
            ? throw new KeyNotFoundException($"User with id {userId} not found.")
            : new Employee
            {
                Email = employee.Email ?? string.Empty,
                Id = employee.Id,
                Firstname = employee.FirstName,
                Lastname = employee.LastName
            };
    }

    public async Task<List<Employee>> GetEmployees()
    {
        var employees = await userManager.GetUsersInRoleAsync("Employee");
        return [.. employees.Select(q => new Employee
        {
            Id = q.Id,
            Email = q.Email ?? string.Empty,
            Firstname = q.FirstName,
            Lastname = q.LastName
        })];
    }
}