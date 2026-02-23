namespace TechShop.ECommerce.Api.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiCore(this IServiceCollection services, IConfiguration config)
    {
        services.AddApplicationServices();
        services.AddInfrastructureServices(config);
        services.AddPersistenceServices(config);
        services.AddIdentityServices(config);

        services.AddControllers();
        services.AddHttpContextAccessor();

        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }
}
