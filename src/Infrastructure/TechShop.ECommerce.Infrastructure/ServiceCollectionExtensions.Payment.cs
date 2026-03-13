namespace TechShop.ECommerce.Infrastructure;


public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddPaymentInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<StripeOptions>()
            .BindConfiguration("StripeSettings")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<IPaymentService, StripePaymentService>();

        return services;
    }
}
