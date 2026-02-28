namespace TechShop.ECommerce.Api.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddOutputCachingPolicies(this IServiceCollection services)
    {
        services.AddOutputCache(options =>
        {
            options.AddPolicy("ProductsList", policy =>
                policy.Cache()
                      .Expire(TimeSpan.FromMinutes(2))
                      .SetLocking(true)
                      .Tag("products")
                      .SetVaryByQuery(
                          "pageNumber",
                          "pageSize",
                          "categoryId",
                          "sortBy",
                          "search"
                      ));

            options.AddPolicy("ProductDetail", policy =>
                policy.Cache()
                      .Expire(TimeSpan.FromMinutes(5))
                      .SetLocking(true)
                      .SetVaryByRouteValue("id"));
        });

        return services;
    }
}
