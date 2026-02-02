using TechShop.ECommerce.Application.Features.Products.Dtos;
using TechShop.ECommerce.Application.Models;

namespace TechShop.ECommerce.Application.Contracts.Persistence;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<IReadOnlyList<ProductDto>> GetAllAsync();
    Task<PagedResult<ProductDto>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
    Task AddAsync(Product product);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
    Task<bool> HasOrdersAsync(int productId);

}
