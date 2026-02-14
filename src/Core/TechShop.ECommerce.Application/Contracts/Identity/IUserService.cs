namespace TechShop.ECommerce.Application.Contracts.Identity;

public interface IUserService
{
    Task<List<Employee>> GetEmployees();
    Task<Employee> GetEmployee(Guid userId);
    public Guid UserId { get; }
}
