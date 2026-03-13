namespace TechShop.ECommerce.Identity;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddJwtAuthenticationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddOptions<JwtOptions>()
            .BindConfiguration("JwtSettings")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer();

        services.ConfigureOptions<JwtBearerOptionsSetup>();

        return services;
    }
}
