namespace TechShop.ECommerce.Application.Contracts.Caching;

public interface ICacheable
{
    bool BypassCache { get; }
    string CacheKey { get; }
    int AbsoluteExpirationInMinutes { get; }

    IEnumerable<string>? Tags { get; }
}
