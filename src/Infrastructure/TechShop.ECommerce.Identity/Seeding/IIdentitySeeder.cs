namespace TechShop.ECommerce.Identity.Seeding;

public interface IIdentitySeeder
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}