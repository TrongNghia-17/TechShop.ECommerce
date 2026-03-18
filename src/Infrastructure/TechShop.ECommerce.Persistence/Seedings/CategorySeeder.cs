using TechShop.ECommerce.Domain.Entities.Catalogs;
using TechShop.ECommerce.Persistence.Context;

namespace TechShop.ECommerce.Persistence.Seedings;

public sealed class CategorySeeder(TechShopDbContext context) : IDataSeeder
{
    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        var hasCategories = await context.Categories.AnyAsync(cancellationToken);

        if (hasCategories)
        {
            return;
        }

        var categories = new[]
        {
            Category.Create("Laptop", "Portable computers for work and gaming."),
            Category.Create("Smartphone", "Modern smartphones and accessories."),
            Category.Create("Tablet", "Tablets for entertainment and productivity."),
            Category.Create("Headphone", "Audio devices for music and calls."),
            Category.Create("Accessory", "Tech accessories and peripherals.")
        };

        await context.Categories.AddRangeAsync(categories, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}