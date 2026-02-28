namespace TechShop.ECommerce.Infrastructure;

public static class InfrastructureServicesRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<EmailSettings>()
            .BindConfiguration("EmailSettings")
            .Validate(s => !string.IsNullOrWhiteSpace(s.ApiKey), "Email ApiKey is required")
            .Validate(s => !string.IsNullOrWhiteSpace(s.FromAddress), "FromAddress is required")
            .ValidateOnStart();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";

            options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
            {
                EndPoints = { options.Configuration },
                AbortOnConnectFail = false,
                ConnectRetry = 3,
                ConnectTimeout = 5000,
                SyncTimeout = 5000
            };
        });

        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }
}
