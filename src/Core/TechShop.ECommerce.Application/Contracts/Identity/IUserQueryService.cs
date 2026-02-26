namespace TechShop.ECommerce.Application.Contracts.Identity;

public interface IUserQueryService
{
    Task<List<Customer>> GetCustomers();
    Task<Customer> GetCustomer(Guid userId);
}
