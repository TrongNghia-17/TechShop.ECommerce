namespace TechShop.ECommerce.Infrastructure.Caching;

public sealed class CacheVersionService(
    ICacheService cacheService)
    : ICacheVersionService
{
    public async Task<int> GetProductsVersionAsync(
        CancellationToken cancellationToken = default)
    {
        var version = await cacheService.GetOrSetAsync(
            CacheKeys.Products.VersionKey,
            () => Task.FromResult(1),
            absoluteExpiration: TimeSpan.FromHours(1));

        return version;
    }
}