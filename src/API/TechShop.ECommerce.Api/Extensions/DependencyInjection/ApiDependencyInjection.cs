using TechShop.ECommerce.Api.Middleware;
using TechShop.ECommerce.Application;
using TechShop.ECommerce.Application.Common.Constants;
using TechShop.ECommerce.Identity.DependencyInjection;
using TechShop.ECommerce.Infrastructure.DependencyInjection;
using TechShop.ECommerce.Persistence.DependencyInjection;

namespace TechShop.ECommerce.Api.Extensions.DependencyInjection;

public static class ApiDependencyInjection
{
    private const string FrontendCorsPolicy = "Frontend";

    public static IServiceCollection AddApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCoreApplicationServices();
        services.AddPersistenceServices(configuration);
        services.AddInfrastructureServices(configuration);

        services.AddIdentityInfrastructure(configuration);
        services.AddCurrentUserContext();
        services.AddJwtAuthentication();

        services.AddAuthorization();

        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddOpenApi();
        services.AddApiOpenTelemetry(configuration);
        services.AddApiResponseCompression();
        services.AddApiCors();
        services.AddApiHangfire(configuration);

        services.AddApplicationInsightsTelemetry();

        return services;
    }

    private static IServiceCollection AddApiResponseCompression(this IServiceCollection services)
    {
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

    private static IServiceCollection AddApiCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(FrontendCorsPolicy, policy =>
            {
                policy.WithOrigins(
                        "http://localhost:5173",
                        "https://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }

    private static IServiceCollection AddApiHangfire(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStrings.Default)
            ?? throw new InvalidOperationException(
                $"Connection string '{ConnectionStrings.Default}' was not found.");

        services.AddHangfire(hangfireConfig =>
        {
            hangfireConfig.UsePostgreSqlStorage(options =>
                options.UseNpgsqlConnection(connectionString));
        });

        return services;
    }
}
