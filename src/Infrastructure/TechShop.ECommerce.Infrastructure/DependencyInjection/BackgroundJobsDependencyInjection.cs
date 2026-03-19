using TechShop.ECommerce.Application.Contracts.Jobs;
using TechShop.ECommerce.Infrastructure.Jobs.Emails;
using TechShop.ECommerce.Infrastructure.Jobs.Orders;
using TechShop.ECommerce.Infrastructure.Jobs.Payments;

namespace TechShop.ECommerce.Infrastructure.DependencyInjection;

public static class BackgroundJobsDependencyInjection
{
    public static IServiceCollection AddBackgroundJobInfrastructure(this IServiceCollection services)
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