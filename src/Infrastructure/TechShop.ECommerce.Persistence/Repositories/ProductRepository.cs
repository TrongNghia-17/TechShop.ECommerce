namespace TechShop.ECommerce.Persistence.Repositories;

public class ProductRepository(TechShopDatabaseContext context)
    : IProductRepository
{
    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await context.Products
            .FirstOrDefaultAsync(p => p.Id == id);
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

    public Task<PagedResult<ProductDto>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Guid? categoryId,
        string? sort,
        CancellationToken token)
    {
        var query = context.Products
            .AsNoTracking();

        if (categoryId is not null)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        var desc = !string.IsNullOrWhiteSpace(sort) && sort.StartsWith("-");
        query = desc
            ? query.OrderByDescending(p => p.Price)
            : query.OrderBy(p => p.Price);

        var dtoQuery = query.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Category.Name));

        return dtoQuery.ToPageResultAsync(pageNumber, pageSize, token, maxPageSize: 100);
    }

    public async Task<CursorPagedResult<ProductFeedItemDto>> GetAllCursorAsync(
       string? search,
       ProductCursor? after,
       int limit,
       CancellationToken token)
    {
        var take = Math.Clamp(limit, 1, 100);

        IQueryable<Product> query = context.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var keyword = search.Trim();

            query = query.Where(p =>
                EF.Property<NpgsqlTsVector>(p, "SearchVector")
                    .Matches(EF.Functions.PlainToTsQuery(search)));
        }

        if (after is not null)
            query = query.Where(p =>
                p.DateCreated < after.DateCreated ||
                (p.DateCreated == after.DateCreated && p.Id < after.Id));

        query = query
            .OrderByDescending(p => p.DateCreated)
            .ThenByDescending(p => p.Id);

        var entities = await query
            .Take(take + 1)
            .Select(p => new ProductFeedItemDto(
                p.Id,
                p.Name,
                p.Price,
                p.Category.Name,
                p.DateCreated
            ))
            .ToListAsync(token);

        var hasMore = entities.Count > take;
        var items = hasMore ? [.. entities.Take(take)] : entities;

        string? nextCursor = null;

        if (hasMore)
        {
            var last = items[^1];
            nextCursor = CursorEncoder.Encode(
                new ProductCursor(last.DateCreated, last.Id));
        }

        return new CursorPagedResult<ProductFeedItemDto>
        {
            Items = items,
            Limit = take,
            NextCursor = nextCursor
        };
    }


    public async Task AddAsync(Product product)
    {
        await context.Products.AddAsync(product);
    }

    public void Delete(Product product)
    {
        context.Products.Remove(product);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await context.Products
            .AnyAsync(p => p.Id == id);
    }

    public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null)
    {
        return await context.Products.AnyAsync(p =>
            p.Name == name &&
            (!excludeId.HasValue || p.Id != excludeId.Value));
    }

    public async Task<bool> HasOrdersAsync(Guid productId)
    {
        return await context.OrderItems
            .AnyAsync(oi => oi.ProductId == productId);
    }

    public async Task UpdatePriceByCategoryAsync(
        Guid categoryId,
        decimal priceMultiplier,
        string modifiedBy,
        CancellationToken token = default)
    {
        await context.Products
            .Where(p => p.CategoryId == categoryId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(p => p.Price, p => p.Price * priceMultiplier)
                .SetProperty(p => p.DateModified, DateTimeOffset.UtcNow)
                .SetProperty(p => p.ModifiedBy, modifiedBy),
                token);
    }

    public async Task<int> DeleteSoftDeletedProductsAsync(
        DateTimeOffset thresholdDate,
        CancellationToken token = default)
    {
        return await context.Products
             .IgnoreQueryFilters()
             .Where(p => p.IsDeleted && p.DateDeleted < thresholdDate)
             .ExecuteDeleteAsync(token);
    }
}
