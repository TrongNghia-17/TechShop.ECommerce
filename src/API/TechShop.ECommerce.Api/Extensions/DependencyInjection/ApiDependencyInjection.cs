using Microsoft.OpenApi;
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

        services.AddUserRequestContext();
        services.AddJwtAuthentication(configuration);

        services.AddAuthorization();

        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                var clientId = configuration["AzureAd:ClientId"];
                var tenantId = configuration["AzureAd:TenantId"];
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes ??= new Dictionary<string, Microsoft.OpenApi.IOpenApiSecurityScheme>();
                
                document.Components.SecuritySchemes.Add("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow // Dùng AuthorizationCode theo chuẩn bảo mật mới nhất của Microsoft (tránh lỗi cấm Implicit)
                        {
                            AuthorizationUrl = new Uri($"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/authorize"),
                            TokenUrl = new Uri($"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { $"api://{clientId}/access_as_user", "Access API" }
                            }
                        }
                    }
                });
                document.Security ??= new List<OpenApiSecurityRequirement>();
                document.Security.Add(new OpenApiSecurityRequirement
                {
                    { new OpenApiSecuritySchemeReference("oauth2"), new List<string> { $"api://{clientId}/access_as_user" } }
                });
                return Task.CompletedTask;
            });
        });

        services.AddApiOpenTelemetry(configuration);
        services.AddApiResponseCompression();
        services.AddApiCors();
        services.AddApiHangfire(configuration);

        //services.AddApplicationInsightsTelemetry();

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
