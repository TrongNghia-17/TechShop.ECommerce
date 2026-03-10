using Microsoft.Extensions.DependencyInjection;

namespace TechShop.ECommerce.Application.BackgroundJobs;

public static class DependencyInjection
{
    public static IServiceCollection AddBackgroundJobApplicationServices(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}
