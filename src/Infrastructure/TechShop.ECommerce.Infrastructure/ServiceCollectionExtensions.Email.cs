namespace TechShop.ECommerce.Infrastructure;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddEmailInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<EmailSettings>()
            .BindConfiguration("EmailSettings")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton(serviceProvider =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<EmailSettings>>().Value;
            return new SendGridClient(settings.ApiKey);
        });

        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IOrderConfirmationEmailBuilder, OrderConfirmationEmailBuilder>();

        return services;
    }
}
