using TechShop.ECommerce.Persistence.DatabaseContext;

namespace TechShop.ECommerce.Persistence.Seeding;

public sealed class ProductSeeder(TechShopDbContext context) : IDataSeeder
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await context.Products.AnyAsync(cancellationToken))
            return;

        var laptopCategory = await context.Categories
            .FirstAsync(c => c.Name == "Laptop", cancellationToken);

        var phoneCategory = await context.Categories
            .FirstAsync(c => c.Name == "Smartphone", cancellationToken);

        var products = new[]
        {
            Product.Create("MacBook Pro 14", 52_000_000, 50, laptopCategory.Id),
            Product.Create("Dell XPS 13", 41_000_000, 50, laptopCategory.Id),
            Product.Create("iPhone 15 Pro", 35_000_000, 50, phoneCategory.Id)
        };

        await context.Products.AddRangeAsync(products, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}