namespace TechShop.ECommerce.Persistence.Seedings;

public static class TechShopSeederRunner
{
    public static async Task SeedAsync(
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var dataSeeders = scope.ServiceProvider.GetServices<IDataSeeder>();

        foreach (var dataSeeder in dataSeeders)
        {
            await dataSeeder.SeedAsync(cancellationToken);
        }
    }
}