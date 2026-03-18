using TechShop.ECommerce.Identity.Seedings;
using TechShop.ECommerce.Identity.Services;

namespace TechShop.ECommerce.Identity.DependencyInjection;

public static class IdentityDependencyInjection
{
    public static IServiceCollection AddIdentityInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(IdentityConstants.DefaultConnectionStringName)
            ?? throw new InvalidOperationException(
                $"Connection string '{IdentityConstants.DefaultConnectionStringName}' was not found.");

        services.AddDbContext<TechShopIdentityDbContext>(options =>
            options.UseNpgsql(
                connectionString,
                npgsqlOptions =>
                    npgsqlOptions.MigrationsAssembly(IdentityConstants.MigrationsAssemblyName)));

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
