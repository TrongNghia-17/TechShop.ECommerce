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

            options.AddInterceptors(
                serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>(),
                serviceProvider.GetRequiredService<SoftDeleteSaveChangesInterceptor>(),
                serviceProvider.GetRequiredService<SlowQueryInterceptor>());
        });

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IDataSeeder, CategorySeeder>();
        services.AddScoped<IDataSeeder, ProductSeeder>();

        return services;
    }
}
