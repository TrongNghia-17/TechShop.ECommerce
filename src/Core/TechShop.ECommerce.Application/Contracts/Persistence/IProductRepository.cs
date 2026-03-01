using TechShop.ECommerce.Application.Common.Paging;
using TechShop.ECommerce.Application.Features.Products.Queries.GetProducts;

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
    Task<PagedResponse<ProductDto>> GetPagedAsync(
        ProductQueryFilter filter,
        CancellationToken token);
    Task AddAsync(Product product);
}
