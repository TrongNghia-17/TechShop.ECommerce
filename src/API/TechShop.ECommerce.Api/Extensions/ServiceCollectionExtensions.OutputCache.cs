namespace TechShop.ECommerce.Api.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddOutputCachingPolicies(this IServiceCollection services)
    {
        services.AddOutputCache(options =>
        {
            options.AddPolicy("ProductsList", policy =>
                policy.Expire(TimeSpan.FromMinutes(2))
                      .Tag("products")
                      .SetVaryByQuery("pageNumber", "pageSize", "categoryId", "sort"));
        });

        return services;
    }
}
