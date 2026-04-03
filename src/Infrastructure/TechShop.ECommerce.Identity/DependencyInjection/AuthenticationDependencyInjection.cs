using Microsoft.Identity.Web;
using TechShop.ECommerce.Application.Common.Configurations.Identity;
using TechShop.ECommerce.Identity.Services;


namespace TechShop.ECommerce.Identity.DependencyInjection;

public static class AuthenticationDependencyInjection
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(configuration.GetSection(AzureAdOptions.SectionName));

        return services;
    }

    public static IServiceCollection AddUserRequestContext(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
