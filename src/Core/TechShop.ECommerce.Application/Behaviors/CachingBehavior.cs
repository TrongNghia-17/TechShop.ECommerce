using TechShop.ECommerce.Application.Contracts.Caching;

namespace TechShop.ECommerce.Application.Behaviors;

public class CachingBehavior<TRequest, TResponse>(
    ILogger<CachingBehavior<TRequest, TResponse>> logger,
    IAppCache cache)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheable
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request.BypassCache)
            return await next(cancellationToken);

        var cacheKey = request.CacheKey;

        var expiration = TimeSpan.FromMinutes(
            request.AbsoluteExpirationInMinutes == 0
                ? 5
                : request.AbsoluteExpirationInMinutes);

        return await cache.GetOrCreateAsync(
            key: cacheKey,
            factory: async cancellationToken =>
            {
                logger.LogInformation("Cache miss: {CacheKey}", cacheKey);
                return await next(cancellationToken);
            },
            expiration,
            tags: request.Tags,
            cancellationToken: cancellationToken);
    }
}