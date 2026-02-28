namespace TechShop.ECommerce.Infrastructure.Caching;

public sealed class RedisCacheService(IDistributedCache cache) : ICacheService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = null,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? absoluteExpiration = null,
        TimeSpan? slidingExpiration = null)
    {
        var options = new DistributedCacheEntryOptions();

        if (absoluteExpiration.HasValue)
            options.SetAbsoluteExpiration(absoluteExpiration.Value);

        if (slidingExpiration.HasValue)
            options.SetSlidingExpiration(slidingExpiration.Value);

        var json = JsonSerializer.Serialize(value, JsonOptions);
        var bytes = Encoding.UTF8.GetBytes(json);

        await cache.SetAsync(key, bytes, options);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var bytes = await cache.GetAsync(key);

        if (bytes is null)
            return default;

        var json = Encoding.UTF8.GetString(bytes);
        return JsonSerializer.Deserialize<T>(json, JsonOptions);
    }

    public async Task<T?> GetOrSetAsync<T>(
        string key,
        Func<Task<T>> factory,
        TimeSpan? absoluteExpiration = null,
        TimeSpan? slidingExpiration = null)
    {
        var bytes = await cache.GetAsync(key);

        if (bytes is not null)
        {
            var json = Encoding.UTF8.GetString(bytes);
            return JsonSerializer.Deserialize<T>(json);
        }

        var value = await factory();

        if (value is not null)
        {
            await SetAsync(key, value, absoluteExpiration, slidingExpiration);
        }

        return value;
    }

    public Task RemoveAsync(string key)
        => cache.RemoveAsync(key);
}
