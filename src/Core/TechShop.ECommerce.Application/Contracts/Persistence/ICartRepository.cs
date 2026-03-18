using TechShop.ECommerce.Domain.Entities.Carts;

namespace TechShop.ECommerce.Application.Contracts.Persistence;

public interface ICartRepository
{
    Task<Cart?> GetByCustomerIdAsync(
        Guid customerId,
        CancellationToken token = default
    );
    Task AddAsync(
        Cart cart,
        CancellationToken token = default
    );
}
