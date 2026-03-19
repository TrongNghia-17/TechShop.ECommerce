using Microsoft.Extensions.DependencyInjection;

namespace TechShop.ECommerce.Application.BackgroundJobs;

public static class BackgroundJobsDependencyInjection
{
    public static IServiceCollection AddBackgroundJobsApplicationServices(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(BackgroundJobsDependencyInjection).Assembly));

        return services;
    }
}
