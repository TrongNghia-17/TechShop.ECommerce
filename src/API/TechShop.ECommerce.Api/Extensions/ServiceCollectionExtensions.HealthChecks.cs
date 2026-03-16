using Microsoft.Extensions.Diagnostics.HealthChecks;
using TechShop.ECommerce.Persistence.DatabaseContext;

namespace TechShop.ECommerce.Api.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiHealthChecks(
        this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck(
                name: "self",
                check: () => HealthCheckResult.Healthy(),
                tags: ["live"])
            .AddDbContextCheck<TechShopDbContext>(
                name: "database",
                failureStatus: HealthStatus.Unhealthy,
                tags: ["ready"]);

        return services;
    }
}
