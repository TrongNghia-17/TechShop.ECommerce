using TechShop.ECommerce.Persistence.DatabaseContext;

namespace TechShop.ECommerce.Persistence.Repositories;

public sealed class ProductRepository(TechShopDbContext context)
    : IProductRepository
{
    public async Task<Product?> GetByIdAsync(
        Guid id,
        CancellationToken token)
    {
        return await context.Products
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken: token);
    }

    public async Task<List<Product>> GetByIdAsync(
        IEnumerable<Guid> ids,
        CancellationToken token)
    {
        return await context.Products
            .Where(p => ids.Contains(p.Id))
            .ToListAsync(cancellationToken: token);
    }

    public async Task<IReadOnlyList<ProductDto>> GetAllAsync()
    {
        return await context.Products
            .AsNoTracking()
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Price,
                p.Category.Name
            ))
            .ToListAsync();
    }

    public async Task<PagedResponse<ProductDto>> GetPagedAsync(
        ProductQueryFilter filter,
        CancellationToken token)
    {
        var pageNumber = Math.Max(1, filter.PageNumber);
        var pageSize = Math.Clamp(filter.PageSize, 1, 50);

        var query = context.Products
            .AsNoTracking()
            .AsQueryable();

        // Filter
        if (filter.CategoryId is not null)
        {
            query = query.Where(p => p.CategoryId == filter.CategoryId);
        }

        // Search
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(p =>
                EF.Functions.ILike(p.Name, $"%{filter.Search}%"));
        }

        // Sort (default Name asc)
        query = ApplySort(query, filter.SortBy);

        // Project
        var dtoQuery = query.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Price,
            p.Category.Name
        ));

        // Count + Pagination
        return await dtoQuery.ToPagedResponseAsync(pageNumber, pageSize, token);
    }


    private static IQueryable<Product> ApplySort(
        IQueryable<Product> query,
        string? sortBy)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return query.OrderBy(p => p.Name);

        var parts = sortBy.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var column = parts[0].ToLower();
        var desc = parts.Length > 1 &&
                   parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

        return column switch
        {
            "price" => desc
                ? query.OrderByDescending(p => p.Price)
                : query.OrderBy(p => p.Price),

            "name" => desc
                ? query.OrderByDescending(p => p.Name)
                : query.OrderBy(p => p.Name),

            _ => query.OrderBy(p => p.Name)
        };
    }

    public async Task AddAsync(Product product)
    {
        await context.Products.AddAsync(product);
    }
}
