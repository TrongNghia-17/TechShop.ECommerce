namespace TechShop.ECommerce.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<SlowQueryInterceptor>();
        services.AddScoped<AuditSaveChangesInterceptor>();
        services.AddScoped<SoftDeleteSaveChangesInterceptor>();

        services.AddDbContext<TechShopDatabaseContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("PostgreSQL"));

            options.UseAsyncSeeding(async (context, _, token) =>
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                if (env == "Development")
                    await TechShopDataSeeder.SeedAsync(
                        (TechShopDatabaseContext)context,
                        token);
            });

            options.UseSeeding((context, _) =>
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                if (env == "Development")
                {
                    TechShopDataSeeder.Seed(
                        (TechShopDatabaseContext)context);
                }
            });

            options.AddInterceptors(
                serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>(),
                serviceProvider.GetRequiredService<SoftDeleteSaveChangesInterceptor>(),
                serviceProvider.GetRequiredService<SlowQueryInterceptor>());
        });

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
