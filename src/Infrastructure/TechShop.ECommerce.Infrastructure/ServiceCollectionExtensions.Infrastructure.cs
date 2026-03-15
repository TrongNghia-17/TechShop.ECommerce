namespace TechShop.ECommerce.Infrastructure;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCachingInfrastructureServices(configuration);
        services.AddEmailInfrastructureServices(configuration);
        services.AddPaymentInfrastructureServices(configuration);
        services.AddStorageServices(configuration);
        services.AddBackgroundJobInfrastructureServices();
        services.AddDocumentServices();

        return services;
    }
}
