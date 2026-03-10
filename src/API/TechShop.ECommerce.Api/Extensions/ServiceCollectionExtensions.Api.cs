namespace TechShop.ECommerce.Api.Extensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddApplicationServices();
        services.AddPersistenceServices(configuration);

        services.AddCachingInfrastructureServices(configuration);
        services.AddEmailInfrastructureServices(configuration);
        services.AddPaymentInfrastructureServices(configuration);
        services.AddBackgroundJobInfrastructureServices();

        services.AddIdentityCoreServices(configuration);
        services.AddHttpCurrentUser();
        services.AddJwtAuthenticationServices(configuration);

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

        services.AddCors(options =>
        {
            options.AddPolicy("all", p => p
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
        });

        services.AddHangfire(hangfireConfig =>
        {
            hangfireConfig.UsePostgreSqlStorage(options =>
                options.UseNpgsqlConnection(
                    configuration.GetConnectionString("DefaultConnection")));
        });

        return services;
    }
}
