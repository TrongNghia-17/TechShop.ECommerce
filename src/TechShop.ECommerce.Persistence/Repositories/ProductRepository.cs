namespace TechShop.ECommerce.Persistence.Repositories;

public class ProductRepository(TechShopDatabaseContext context)
    : GenericRepository<Product>(context), IProductRepository
{
    public async Task<bool> IsProductUnique(string name)
    {
        var exists = await _context.Products
             .AnyAsync(p => p.Name == name);

        return !exists;
    }
}
