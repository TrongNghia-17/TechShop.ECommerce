namespace TechShop.ECommerce.Persistence.Providers.Postgres;

public static class PostgresPersistenceRegistration
{
    public static IServiceCollection AddPostgresPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TechShopDatabaseContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("Postgres")));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
