namespace TechShop.ECommerce.Application.Contracts.Persistence;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}
