using TechShop.ECommerce.Application.Common.Configurations.PaymentGateway;
using TechShop.ECommerce.Infrastructure.PaymentGateway;

namespace TechShop.ECommerce.Infrastructure.DependencyInjection;


public static class PaymentDependencyInjection
{
    public static IServiceCollection AddPaymentInfrastructure(this IServiceCollection services)
    {
        services.AddOptions<StripeOptions>()
            .BindConfiguration(StripeOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<IPaymentService, StripePaymentService>();

        return services;
    }
}
