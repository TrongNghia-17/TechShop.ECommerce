namespace TechShop.ECommerce.Identity.Seedings;

public static class IdentitySeederRunner
{
    public static async Task SeedAsync(
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var identitySeeders = scope.ServiceProvider.GetServices<IIdentitySeeder>();

        foreach (var identitySeeder in identitySeeders)
        {
            await identitySeeder.SeedAsync(cancellationToken);
        }
    }
}