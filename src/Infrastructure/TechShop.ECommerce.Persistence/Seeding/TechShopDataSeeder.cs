namespace TechShop.ECommerce.Persistence.Seeding;

public static class TechShopDataSeeder
{
    public static async Task SeedAsync(
        TechShopDatabaseContext context,
        CancellationToken token)
    {
        if (await context.Categories.AnyAsync(token)) return;

        var laptopCategory = Category.Create("Laptop");
        var phoneCategory = Category.Create("Smartphone");

        var products = new[]
        {
            Product.Create("MacBook Pro 14", 52_000_000, laptopCategory.Id),
            Product.Create("Dell XPS 13", 41_000_000, laptopCategory.Id),
            Product.Create("iPhone 15 Pro", 35_000_000, phoneCategory.Id)
        };

        context.AddRange(laptopCategory, phoneCategory);
        context.AddRange(products);

        await context.SaveChangesAsync(token);
    }

    public static void Seed(TechShopDatabaseContext context)
    {
        if (context.Categories.Any()) return;

        var laptopCategory = Category.Create("Laptop");
        var phoneCategory = Category.Create("Smartphone");

        var products = new[]
        {
        Product.Create("MacBook Pro 14", 52_000_000, laptopCategory.Id),
        Product.Create("Dell XPS 13", 41_000_000, laptopCategory.Id),
        Product.Create("iPhone 15 Pro", 35_000_000, phoneCategory.Id)
    };

        context.AddRange(laptopCategory, phoneCategory);
        context.AddRange(products);

        context.SaveChanges();
    }
}
