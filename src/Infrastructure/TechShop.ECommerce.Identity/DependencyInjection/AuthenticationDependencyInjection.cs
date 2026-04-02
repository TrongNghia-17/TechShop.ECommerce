using Microsoft.Identity.Web;
using TechShop.ECommerce.Identity.Services;


namespace TechShop.ECommerce.Identity.DependencyInjection;

public static class AuthenticationDependencyInjection
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options => 
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"));

        return services;
    }

    public static IServiceCollection AddUserRequestContext(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
