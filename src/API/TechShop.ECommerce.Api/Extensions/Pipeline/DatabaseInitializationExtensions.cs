using TechShop.ECommerce.Identity.Context;
using TechShop.ECommerce.Identity.Seedings;
using TechShop.ECommerce.Persistence.Context;
using TechShop.ECommerce.Persistence.Seedings;

namespace TechShop.ECommerce.Api.Extensions.Pipeline;

public static class DatabaseInitializationExtensions
{
    public static async Task<WebApplication> ApplyDevelopmentDatabaseInitializationAsync(
        this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return app;
        }

        await using var scope = app.Services.CreateAsyncScope();
        var serviceProvider = scope.ServiceProvider;
        var cancellationToken = CancellationToken.None;

        var appDbContext = serviceProvider.GetRequiredService<TechShopDbContext>();
        await appDbContext.Database.MigrateAsync(cancellationToken);

        var identityDbContext = serviceProvider.GetRequiredService<TechShopIdentityDbContext>();
        await identityDbContext.Database.MigrateAsync(cancellationToken);

        await IdentitySeederRunner.SeedAsync(serviceProvider, cancellationToken);
        await TechShopSeederRunner.SeedAsync(serviceProvider, cancellationToken);

        return app;
    }
}