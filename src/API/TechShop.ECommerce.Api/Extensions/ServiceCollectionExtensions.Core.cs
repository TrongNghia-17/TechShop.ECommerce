namespace TechShop.ECommerce.Api.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiCore(this IServiceCollection services, IConfiguration config)
    {
        services.AddApplicationServices();
        services.AddInfrastructureServices(config);
        services.AddPersistenceServices(config);
        services.AddIdentityServices(config);

        services.AddAuthorization();

        services.AddHttpContextAccessor();

        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddOpenApi();

        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<BrotliCompressionProvider>();
        });

        services.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Optimal;
        });

        return services;
    }
}
