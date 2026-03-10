namespace TechShop.ECommerce.Identity;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityCoreServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TechShopIdentityDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions =>
                    npgsqlOptions.MigrationsAssembly("TechShop.ECommerce.Migrations")));

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<TechShopIdentityDbContext>()
            .AddDefaultTokenProviders();

        services.AddTransient<IUserQueryService, UserQueryService>();
        services.AddScoped<IIdentityService, IdentityService>();

        services.AddScoped<IIdentitySeeder, RoleSeeder>();
        services.AddScoped<IIdentitySeeder, UserSeeder>();

        return services;
    }

    public static IServiceCollection AddHttpCurrentUser(
        this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
