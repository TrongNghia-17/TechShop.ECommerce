using TechShop.ECommerce.Application.Features.Products.Dtos;
using TechShop.ECommerce.Persistence.Extensions;

namespace TechShop.ECommerce.Persistence.Repositories;

public class ProductRepository(TechShopDatabaseContext context)
    : IProductRepository
{
    public async Task<Product?> GetByIdAsync(int id)
    {
        return await context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
    }

    public async Task<IReadOnlyList<ProductDto>> GetAllAsync()
    {
        return await context.Products
            .AsNoTracking()
            .Where(p => !p.IsDeleted)
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Price,
                p.Category.Name
            ))
            .ToListAsync();
    }

    public Task<PagedResult<ProductDto>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = context.Products
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Price,
                p.Category.Name
            ));

        return query.ToPageResultAsync(pageNumber, pageSize, cancellationToken, maxPageSize: 100);
    }

    public async Task AddAsync(Product product)
    {
        await context.Products.AddAsync(product);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await context.Products
            .AnyAsync(p => p.Id == id && !p.IsDeleted);
    }

    public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
    {
        return await context.Products.AnyAsync(p =>
            p.Name == name &&
            !p.IsDeleted &&
            (!excludeId.HasValue || p.Id != excludeId.Value));
    }

    public async Task<bool> HasOrdersAsync(int productId)
    {
        return await context.OrderItems
            .AnyAsync(oi => oi.ProductId == productId);
    }
}
