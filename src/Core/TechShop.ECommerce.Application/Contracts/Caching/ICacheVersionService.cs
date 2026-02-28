namespace TechShop.ECommerce.Application.Contracts.Caching;

public interface ICacheVersionService
{
    Task<int> GetProductsVersionAsync(CancellationToken cancellationToken = default);
}
