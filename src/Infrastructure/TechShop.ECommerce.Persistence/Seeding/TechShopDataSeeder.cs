namespace TechShop.ECommerce.Persistence.Seeding;

public static class TechShopDataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        var seeders = serviceProvider.GetServices<IDataSeeder>();

        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync(cancellationToken);
        }
    }
}