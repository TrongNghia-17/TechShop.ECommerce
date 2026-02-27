namespace TechShop.ECommerce.Application.Contracts.Persistence;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(
        Guid id,
        CancellationToken token = default
    );
    Task<List<Product>> GetByIdAsync(
        IEnumerable<Guid> ids,
        CancellationToken token);
    Task<IReadOnlyList<ProductDto>> GetAllAsync();
    Task<PagedResult<ProductDto>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Guid? categoryId,
        string? sort,
        CancellationToken token);
    Task AddAsync(Product product);
}
