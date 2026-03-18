namespace TechShop.ECommerce.Persistence.Seeding;

public interface IDataSeeder
{
    Task SeedAsync(CancellationToken cancellationToken);
}