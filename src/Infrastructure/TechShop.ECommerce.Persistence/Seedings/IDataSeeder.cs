namespace TechShop.ECommerce.Persistence.Seedings;

public interface IDataSeeder
{
    Task SeedAsync(CancellationToken cancellationToken);
}