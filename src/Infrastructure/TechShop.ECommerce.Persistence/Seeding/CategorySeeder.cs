using TechShop.ECommerce.Persistence.DatabaseContext;

namespace TechShop.ECommerce.Persistence.Seeding;

public sealed class CategorySeeder(TechShopDbContext context) : IDataSeeder
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await context.Categories.AnyAsync(c => c.Name == "Laptop", cancellationToken))
            return;

        var categories = new[]
        {
            Category.Create("Laptop"),
            Category.Create("Smartphone")
        };

        await context.Categories.AddRangeAsync(categories, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}