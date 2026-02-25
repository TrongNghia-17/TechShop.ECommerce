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
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

        return services;
    }
}
