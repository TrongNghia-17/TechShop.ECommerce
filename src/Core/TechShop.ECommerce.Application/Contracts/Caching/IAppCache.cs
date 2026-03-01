namespace TechShop.ECommerce.Application.Contracts.Caching;

public interface IAppCache
{
    Task<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan expiration,
        IEnumerable<string>? tags,
        CancellationToken cancellationToken);

    Task RemoveByTagAsync(string tag, CancellationToken cancellationToken);
}
