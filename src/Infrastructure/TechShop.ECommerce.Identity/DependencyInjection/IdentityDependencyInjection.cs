using TechShop.ECommerce.Identity.Entities;
using TechShop.ECommerce.Identity.Seedings;
using TechShop.ECommerce.Identity.Services;

namespace TechShop.ECommerce.Identity.DependencyInjection;

public static class IdentityDependencyInjection
{
    public static IServiceCollection AddIdentityInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStrings.Default)
            ?? throw new InvalidOperationException(
                $"Connection string '{ConnectionStrings.Default}' was not found.");

        services.AddDbContext<TechShopIdentityDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<TechShopIdentityDbContext>()
            .AddDefaultTokenProviders();

        services.AddTransient<IUserQueryService, UserQueryService>();
        services.AddScoped<IIdentityService, IdentityService>();

        services.AddScoped<IIdentitySeeder, RoleSeeder>();
        services.AddScoped<IIdentitySeeder, UserSeeder>();

        return services;
    }

    public static IServiceCollection AddCurrentUserContext(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
