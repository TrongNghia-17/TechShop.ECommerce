namespace TechShop.ECommerce.Identity.Seeding;

public static class IdentitySeederRunner
{
    public static async Task SeedAsync(IServiceProvider services,
        CancellationToken cancellationToken = default)
    {
        var seeders = services.GetServices<IIdentitySeeder>();

        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync(cancellationToken);
        }
    }
}