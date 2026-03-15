namespace TechShop.ECommerce.Persistence.Repositories;

public sealed class ProductRepository(
    TechShopDbContext context)
    : IProductRepository
{
    public async Task<Product?> GetByIdAsync(
        Guid id,
        CancellationToken token)
    {
        return await context.Products
            .FirstOrDefaultAsync(
                product => product.Id == id,
                cancellationToken: token);
    }

    public async Task<List<Product>> GetByIdAsync(
        IEnumerable<Guid> ids,
        CancellationToken token)
    {
        return await context.Products
            .Where(product => ids.Contains(product.Id))
            .ToListAsync(cancellationToken: token);
    }

    public async Task<IReadOnlyList<GetProductsProjection>> GetAllAsync()
    {
        return await context.Products
            .AsNoTracking()
            .OrderBy(product => product.Name)
            .Select(product => new GetProductsProjection(
                product.Id,
                product.Name,
                product.Price,
                product.Category.Name,
                product.MainImageBlobName))
            .ToListAsync();
    }

    public async Task<PagedResponse<GetProductsProjection>> GetPagedAsync(
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

        var totalRecords = await query.CountAsync(token);

        // Project
        var data = await query
            .ApplyPagination(pageNumber, pageSize)
            .Select(product => new GetProductsProjection(
                product.Id,
                product.Name,
                product.Price,
                product.Category.Name,
                product.MainImageBlobName))
            .ToListAsync(token);

        // Count + Pagination
        return new PagedResponse<GetProductsProjection>
        {
            Data = data,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
        };
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
