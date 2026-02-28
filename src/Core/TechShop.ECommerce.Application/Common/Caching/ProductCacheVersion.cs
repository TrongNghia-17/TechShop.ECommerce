namespace TechShop.ECommerce.Application.Common.Caching;

public sealed class ProductCacheVersion(IDistributedCache cache)
{
    public async Task<int> GetAsync(CancellationToken cancellationToken = default)
    {
        var value = await cache.GetStringAsync(CacheKeys.Products.VersionKey, cancellationToken);
        if (int.TryParse(value, out var version) && version > 0)
            return version;

        await cache.SetStringAsync(CacheKeys.Products.VersionKey, "1", cancellationToken);
        return 1;
    }

    public async Task BumpAsync(CancellationToken cancellationToken = default)
    {
        var current = await GetAsync(cancellationToken);
        await cache.SetStringAsync(CacheKeys.Products.VersionKey, (current + 1).ToString(), cancellationToken);
    }
}