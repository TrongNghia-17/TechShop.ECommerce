using TechShop.ECommerce.Application.Features.Products.Queries.GetProducts;
using TechShop.ECommerce.Domain.Entities.Catalogs;

namespace TechShop.ECommerce.Application.Contracts.Persistence;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken token);
    Task<List<Product>> GetByIdAsync(IEnumerable<Guid> ids, CancellationToken token);
    Task<IReadOnlyList<GetProductsProjection>> GetAllAsync();
    Task<PagedResponse<GetProductsProjection>> GetPagedAsync(
        ProductQueryFilter filter,
        CancellationToken token);
    Task AddAsync(Product product);
}
