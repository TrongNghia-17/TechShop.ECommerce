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
    Task<CursorPagedResult<ProductFeedItemDto>> GetAllCursorAsync(
        string? search,
        ProductCursor? after,
        int pageSize,
        CancellationToken token);
    Task AddAsync(Product product);
    void Delete(Product product);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null);
    Task<bool> HasOrdersAsync(Guid productId);

    Task UpdatePriceByCategoryAsync(
        Guid categoryId,
        decimal priceMultiplier,
        Guid modifiedBy,
        CancellationToken token = default);

    Task<int> DeleteSoftDeletedProductsAsync(
        DateTimeOffset thresholdDate,
        CancellationToken token = default);
}
