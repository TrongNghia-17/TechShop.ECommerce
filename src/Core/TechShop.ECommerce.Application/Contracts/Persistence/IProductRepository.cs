using TechShop.ECommerce.Application.Common.Paging;
using TechShop.ECommerce.Application.Features.Products.GetProducts;
using TechShop.ECommerce.Domain.Entities.Catalogs;

namespace TechShop.ECommerce.Application.Contracts.Persistence;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken token);
    Task<List<Product>> GetByIdAsync(IEnumerable<Guid> ids, CancellationToken token);
    Task<IReadOnlyList<GetProductsProjection>> GetAllAsync();
    Task<IReadOnlyList<Product>> GetAllForIngestionAsync(CancellationToken token);
    Task<PagedResponse<GetProductsProjection>> GetPagedAsync(
        ProductQueryFilter filter,
        CancellationToken token);
    Task AddAsync(Product product);
}
