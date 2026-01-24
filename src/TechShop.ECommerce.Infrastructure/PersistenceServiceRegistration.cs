using TechShop.ECommerce.Persistence.DatabaseContext;

namespace TechShop.ECommerce.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TechShopDatabaseContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("TechShopDatabaseConnectionString"));
        });

        return services;
    }
}
