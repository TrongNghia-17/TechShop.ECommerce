namespace TechShop.ECommerce.Application.Contracts.Identity;

public interface IUserService
{
    Task<List<Customer>> GetCustomers();
    Task<Customer> GetCustomer(Guid userId);
    public Guid UserId { get; }
}
