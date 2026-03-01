namespace TechShop.ECommerce.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<EmailSettings>()
            .BindConfiguration("EmailSettings")
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

        return services;
    }
}
