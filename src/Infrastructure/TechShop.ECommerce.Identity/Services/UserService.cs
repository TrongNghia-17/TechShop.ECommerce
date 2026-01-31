using Microsoft.AspNetCore.Http;

namespace TechShop.ECommerce.Identity.Services;

public class UserService(
    UserManager<ApplicationUser> userManager,
    IHttpContextAccessor contextAccessor)
    : IUserService
{
    public string UserId { get => contextAccessor.HttpContext?.User?.FindFirstValue("uid"); }

    public async Task<Employee> GetEmployee(string userId)
    {
        var employee = await userManager.FindByIdAsync(userId);
        return new Employee
        {
            Email = employee.Email,
            Id = employee.Id,
            Firstname = employee.FirstName,
            Lastname = employee.LastName
        };
    }

    public async Task<List<Employee>> GetEmployees()
    {
        var employees = await userManager.GetUsersInRoleAsync("Employee");
        return employees.Select(q => new Employee
        {
            Id = q.Id,
            Email = q.Email,
            Firstname = q.FirstName,
            Lastname = q.LastName
        }).ToList();
    }
}