using TechShop.ECommerce.Infrastructure.Emails;

namespace TechShop.ECommerce.Infrastructure.DependencyInjection;

public static class EmailDependencyInjection
{
    public static IServiceCollection AddEmailInfrastructure(this IServiceCollection services)
    {
        services.AddOptions<EmailOptions>()
            .BindConfiguration(EmailOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton(serviceProvider =>
        {
            var emailOptions = serviceProvider
                .GetRequiredService<IOptions<EmailOptions>>()
                .Value;

            return new SendGridClient(emailOptions.ApiKey);
        });

        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IOrderConfirmationEmailBuilder, OrderConfirmationEmailBuilder>();

        return services;
    }
}