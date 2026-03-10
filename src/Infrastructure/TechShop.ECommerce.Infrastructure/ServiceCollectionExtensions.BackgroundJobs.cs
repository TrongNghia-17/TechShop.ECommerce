using TechShop.ECommerce.Infrastructure.BackgroundJobs.Orders;

namespace TechShop.ECommerce.Infrastructure;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackgroundJobInfrastructureServices(
        this IServiceCollection services)
    {
        services.AddScoped<IEmailJobs, HangfireEmailJobs>();
        services.AddScoped<IHangfireEmailJobExecutor, HangfireEmailJobExecutor>();

        services.AddScoped<IHangfireOrderMaintenanceJobExecutor, HangfireOrderMaintenanceJobExecutor>();

        return services;
    }
}