using TechShop.ECommerce.Application.BackgroundJobs.Emails;
using TechShop.ECommerce.Application.Common.Emails;
using TechShop.ECommerce.Application.Contracts.PaymentGateway;
using TechShop.ECommerce.Infrastructure.BackgroundJobs.Emails;
using TechShop.ECommerce.Infrastructure.Emails;
using TechShop.ECommerce.Infrastructure.PaymentGateway;

namespace TechShop.ECommerce.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<EmailSettings>()
            .BindConfiguration("EmailSettings")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton<SendGridClient>(serviceProvider =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<EmailSettings>>().Value;
            return new SendGridClient(settings.ApiKey);
        });

        services.AddOptions<StripeSettings>()
            .BindConfiguration("StripeSettings")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";

            options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
            {
                EndPoints = { options.Configuration }
            };
        });

        services.AddHybridCache();

        services.AddScoped<IEmailSender, EmailSender>();

        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

        services.AddScoped<IAppCache, AppHybridCache>();

        services.AddScoped<IPaymentService, StripePaymentService>();

        services.AddScoped<IEmailJobs, HangfireEmailJobs>();
        services.AddScoped<IHangfireEmailJobExecutor, HangfireEmailJobExecutor>();

        return services;
    }
}
