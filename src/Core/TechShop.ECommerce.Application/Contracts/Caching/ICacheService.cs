namespace TechShop.ECommerce.Application.Contracts.Caching;

public interface ICacheService
{
    Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? absoluteExpiration = null,
        TimeSpan? slidingExpiration = null);

    Task<T?> GetAsync<T>(string key);

    Task<T?> GetOrSetAsync<T>(
        string key,
        Func<Task<T>> factory,
        TimeSpan? absoluteExpiration = null,
        TimeSpan? slidingExpiration = null);

    Task RemoveAsync(string key);
}
