using TechShop.ECommerce.Application.Contracts.Jobs;
using TechShop.ECommerce.Infrastructure.Jobs.Orders;
using TechShop.ECommerce.Infrastructure.Jobs.Payments;

namespace TechShop.ECommerce.Infrastructure;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackgroundJobInfrastructureServices(
        this IServiceCollection services)
    {
        services.AddScoped<IEmailJobs, HangfireEmailJobs>();
        services.AddScoped<IHangfireEmailJobExecutor, HangfireEmailJobExecutor>();

        services.AddScoped<IPaymentJobs, HangfirePaymentJobs>();
        services.AddScoped<IHangfirePaymentJobExecutor, HangfirePaymentJobExecutor>();

        services.AddScoped<IStripeWebhookJobs, HangfireStripeWebhookJobs>();
        services.AddScoped<IHangfireStripeWebhookJobExecutor, HangfireStripeWebhookJobExecutor>();

        services.AddScoped<IHangfireOrderMaintenanceJobExecutor, HangfireOrderMaintenanceJobExecutor>();

        return services;
    }
}