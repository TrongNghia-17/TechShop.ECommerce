namespace TechShop.ECommerce.Infrastructure;


public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddPaymentInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<StripeSettings>()
            .BindConfiguration("StripeSettings")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<IPaymentService, StripePaymentService>();

        return services;
    }
}
