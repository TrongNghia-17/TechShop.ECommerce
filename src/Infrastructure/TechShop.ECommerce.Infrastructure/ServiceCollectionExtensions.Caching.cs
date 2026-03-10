using StackExchange.Redis;

namespace TechShop.ECommerce.Infrastructure;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddCachingInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";

            options.ConfigurationOptions = new ConfigurationOptions
            {
                EndPoints = { options.Configuration }
            };
        });

        services.AddHybridCache();

        services.AddScoped<IAppCache, AppHybridCache>();

        return services;
    }
}