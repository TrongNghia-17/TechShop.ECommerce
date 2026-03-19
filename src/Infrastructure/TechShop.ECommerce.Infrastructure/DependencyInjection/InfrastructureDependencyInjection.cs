namespace TechShop.ECommerce.Infrastructure.DependencyInjection;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCachingInfrastructure(configuration);
        services.AddEmailInfrastructure();
        services.AddPaymentInfrastructure();
        services.AddStorageInfrastructure();
        services.AddBackgroundJobInfrastructure();
        services.AddDocumentInfrastructure();

        return services;
    }
}
