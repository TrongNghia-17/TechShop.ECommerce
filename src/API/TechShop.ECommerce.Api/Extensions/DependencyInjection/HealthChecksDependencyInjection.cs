using Microsoft.Extensions.Diagnostics.HealthChecks;
using TechShop.ECommerce.Persistence.Context;

namespace TechShop.ECommerce.Api.Extensions.DependencyInjection;

public static class HealthChecksDependencyInjection
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
