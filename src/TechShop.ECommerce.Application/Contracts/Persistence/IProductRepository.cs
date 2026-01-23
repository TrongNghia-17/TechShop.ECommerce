namespace TechShop.ECommerce.Application.Contracts.Persistence;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<bool> IsProductUnique(string name);
}
