namespace TechShop.ECommerce.Infrastructure;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackgroundJobInfrastructureServices(
        this IServiceCollection services)
    {
        services.AddScoped<IEmailJobs, HangfireEmailJobs>();
        services.AddScoped<IHangfireEmailJobExecutor, HangfireEmailJobExecutor>();

        return services;
    }
}