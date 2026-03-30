using TechShop.ECommerce.Application.Common.Constants;
using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Persistence.Context;
using TechShop.ECommerce.Persistence.Interceptors;
using TechShop.ECommerce.Persistence.Repositories;
using TechShop.ECommerce.Persistence.Seedings;

namespace TechShop.ECommerce.Persistence.DependencyInjection;

public static class PersistenceDependencyInjection
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStrings.Default)
            ?? throw new InvalidOperationException(
                $"Connection string '{ConnectionStrings.Default}' was not found.");

        services.AddScoped<AuditSaveChangesInterceptor>();
        services.AddScoped<SoftDeleteSaveChangesInterceptor>();

        services.AddDbContext<TechShopDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(connectionString, o => o.UseVector());
            options.AddInterceptors(
                serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>(),
                serviceProvider.GetRequiredService<SoftDeleteSaveChangesInterceptor>());
        });

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IPGVectorRepository, PGVectorRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IDataSeeder, CategorySeeder>();
        services.AddScoped<IDataSeeder, ProductSeeder>();

        return services;
    }
}
