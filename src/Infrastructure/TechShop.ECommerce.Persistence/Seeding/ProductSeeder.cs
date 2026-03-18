namespace TechShop.ECommerce.Persistence.Seeding;

public sealed class ProductSeeder(TechShopDbContext context) : IDataSeeder
{
    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        var hasProducts = await context.Products.AnyAsync(cancellationToken);

        if (hasProducts)
        {
            return;
        }

        var laptopCategory = await context.Categories
            .FirstAsync(category => category.Name == "Laptop", cancellationToken);

        var smartphoneCategory = await context.Categories
            .FirstAsync(category => category.Name == "Smartphone", cancellationToken);

        var tabletCategory = await context.Categories
            .FirstAsync(category => category.Name == "Tablet", cancellationToken);

        var headphoneCategory = await context.Categories
            .FirstAsync(category => category.Name == "Headphone", cancellationToken);

        var accessoryCategory = await context.Categories
            .FirstAsync(category => category.Name == "Accessory", cancellationToken);

        var products = new[]
        {
            Product.Create(
                name: "MacBook Pro 14",
                price: 1999.99m,
                stockQuantity: 10,
                categoryId: laptopCategory.Id,
                summary: "Powerful Apple laptop.",
                description: "MacBook Pro 14-inch with M-series chip and Retina display."),

            Product.Create(
                name: "Dell XPS 13",
                price: 1499.99m,
                stockQuantity: 12,
                categoryId: laptopCategory.Id,
                summary: "Compact ultrabook.",
                description: "Dell XPS 13 with premium design and strong performance."),

            Product.Create(
                name: "iPhone 15",
                price: 999.99m,
                stockQuantity: 25,
                categoryId: smartphoneCategory.Id,
                summary: "Latest Apple smartphone.",
                description: "iPhone 15 with advanced camera and performance improvements."),

            Product.Create(
                name: "Samsung Galaxy S24",
                price: 899.99m,
                stockQuantity: 20,
                categoryId: smartphoneCategory.Id,
                summary: "Flagship Android phone.",
                description: "Samsung Galaxy S24 with premium display and camera system."),

            Product.Create(
                name: "iPad Air",
                price: 699.99m,
                stockQuantity: 15,
                categoryId: tabletCategory.Id,
                summary: "Lightweight Apple tablet.",
                description: "iPad Air for work, study, and entertainment."),

            Product.Create(
                name: "Sony WH-1000XM5",
                price: 399.99m,
                stockQuantity: 18,
                categoryId: headphoneCategory.Id,
                summary: "Noise-cancelling headphones.",
                description: "Sony premium wireless headphones with excellent ANC."),

            Product.Create(
                name: "Logitech MX Master 3S",
                price: 99.99m,
                stockQuantity: 30,
                categoryId: accessoryCategory.Id,
                summary: "Advanced wireless mouse.",
                description: "Ergonomic productivity mouse for professional workflows.")
        };

        await context.Products.AddRangeAsync(products, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}