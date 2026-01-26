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

    public async Task<List<Product>> GetProductsWithDetailsAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .Where(p => !p.IsDeleted)
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdWithDetailsAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .Where(p => !p.IsDeleted)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
