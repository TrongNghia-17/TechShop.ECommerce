namespace TechShop.ECommerce.Application.Contracts.Persistence;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<ProductDto>> GetAllAsync();
    Task<PagedResult<ProductDto>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Guid? categoryId,
        string? sort,
        CancellationToken token);
    Task AddAsync(Product product);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null);
    Task<bool> HasOrdersAsync(Guid productId);

}
