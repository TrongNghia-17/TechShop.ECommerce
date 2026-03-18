using TechShop.ECommerce.Application.Common.Paging;
using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Application.Features.Products.Queries.GetProducts;
using TechShop.ECommerce.Persistence.Context;

namespace TechShop.ECommerce.Persistence.Repositories;

public sealed class ProductRepository(TechShopDbContext context) : IProductRepository
{
    public async Task<Product?> GetByIdAsync(
        Guid productId,
        CancellationToken cancellationToken)
    {
        return await context.Products
            .FirstOrDefaultAsync(
                product => product.Id == productId,
                cancellationToken);
    }

    public async Task<List<Product>> GetByIdAsync(
        IEnumerable<Guid> productIds,
        CancellationToken cancellationToken)
    {
        return await context.Products
            .Where(product => productIds.Contains(product.Id))
            .ToListAsync(cancellationToken);
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
        CancellationToken cancellationToken)
    {
        var pageNumber = Math.Max(1, filter.PageNumber);
        var pageSize = Math.Clamp(filter.PageSize, 1, 50);

        IQueryable<Product> query = context.Products
            .AsNoTracking();

        if (filter.CategoryId is not null)
        {
            query = query.Where(product => product.CategoryId == filter.CategoryId);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var searchTerm = filter.Search.Trim();

            query = query.Where(product =>
                EF.Functions.ILike(product.Name, $"%{searchTerm}%"));
        }

        query = ApplySort(query, filter.SortBy);

        var totalRecords = await query.CountAsync(cancellationToken);

        var data = await query
            .ApplyPagination(pageNumber, pageSize)
            .Select(product => new GetProductsProjection(
                product.Id,
                product.Name,
                product.Price,
                product.Category.Name,
                product.MainImageBlobName))
            .ToListAsync(cancellationToken);

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
        {
            return query.OrderBy(product => product.Name);
        }

        var parts = sortBy.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var column = parts[0].ToLowerInvariant();
        var isDescending = parts.Length > 1 &&
            parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

        return column switch
        {
            "price" => isDescending
                ? query.OrderByDescending(product => product.Price)
                : query.OrderBy(product => product.Price),

            "name" => isDescending
                ? query.OrderByDescending(product => product.Name)
                : query.OrderBy(product => product.Name),

            _ => query.OrderBy(product => product.Name)
        };
    }

    public async Task AddAsync(Product product)
    {
        await context.Products.AddAsync(product);
    }
}
