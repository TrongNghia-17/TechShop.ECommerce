using TechShop.ECommerce.Application.Common.Constants;
using TechShop.ECommerce.Infrastructure.Caching;

namespace TechShop.ECommerce.Infrastructure.DependencyInjection;

public static class CachingDependencyInjection
{
    public static IServiceCollection AddCachingInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString(ConnectionStrings.Redis)
            ?? throw new InvalidOperationException(
                $"Connection string '{ConnectionStrings.Redis}' was not found.");

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
        });

        services.AddHybridCache();

        services.AddScoped<IAppCache, AppHybridCache>();

        return services;
    }
}