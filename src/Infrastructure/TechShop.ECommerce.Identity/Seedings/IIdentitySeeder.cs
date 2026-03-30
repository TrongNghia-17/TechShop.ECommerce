namespace TechShop.ECommerce.Identity.Seedings;

public interface IIdentitySeeder
{
    Task SeedAsync(CancellationToken cancellationToken);
}