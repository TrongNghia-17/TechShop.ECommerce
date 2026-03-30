namespace TechShop.ECommerce.Infrastructure.Caching;

public class AppHybridCache(HybridCache cache) : IAppCache
{
    public async Task<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan expiration,
        IEnumerable<string>? tags,
        CancellationToken cancellationToken)
    {
        return await cache.GetOrCreateAsync<object?, T>(
            key: key,
            state: null,
            factory: async (_, cancellationToken) =>
                await factory(cancellationToken),
            options: new HybridCacheEntryOptions
            {
                Expiration = expiration
            },
            tags: tags,
            cancellationToken: cancellationToken);
    }

    public Task RemoveByTagAsync(string tag, CancellationToken cancellationToken)
        => cache.RemoveByTagAsync(tag, cancellationToken).AsTask();
}
