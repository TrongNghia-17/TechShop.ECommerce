namespace TechShop.ECommerce.Application.Behaviors;

public class CachingBehavior<TRequest, TResponse>(
    ILogger<CachingBehavior<TRequest, TResponse>> logger,
    IDistributedCache cache,
    ProductCacheVersion productVersion
    )
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheable
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        TResponse response;
        if (request.BypassCache) return await next();

        var cacheKey = request.CacheKey;

        if (cacheKey.Contains(":products:", StringComparison.Ordinal))
        {
            var v = await productVersion.GetAsync(cancellationToken);
            cacheKey = cacheKey.Replace("techshop:", $"techshop:v{v}:", StringComparison.Ordinal);
        }

        async Task<TResponse> GetResponseAndAddToCache()
        {
            response = await next();
            if (response != null)
            {
                var slidingExpiration = request.SlidingExpirationInMinutes == 0 ? 30 : request.SlidingExpirationInMinutes;
                var absoluteExpiration = request.AbsoluteExpirationInMinutes == 0 ? 60 : request.AbsoluteExpirationInMinutes;
                var options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(slidingExpiration))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(absoluteExpiration));

                var serializedData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));
                await cache.SetAsync(cacheKey, serializedData, options, cancellationToken);
            }
            return response;
        }

        var cachedResponse = await cache.GetAsync(cacheKey, cancellationToken);
        if (cachedResponse != null)
        {
            response = JsonSerializer.Deserialize<TResponse>(Encoding.UTF8.GetString(cachedResponse))!;
            logger.LogInformation("fetched from cache with key : {CacheKey}", cacheKey);
            await cache.RefreshAsync(cacheKey, cancellationToken);
        }
        else
        {
            response = await GetResponseAndAddToCache();
            logger.LogInformation("added to cache with key : {CacheKey}", cacheKey);
        }
        return response;
    }
}
