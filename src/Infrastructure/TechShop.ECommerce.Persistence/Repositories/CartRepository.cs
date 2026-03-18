using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Persistence.Context;

namespace TechShop.ECommerce.Persistence.Repositories;

public sealed class CartRepository(TechShopDbContext context) : ICartRepository
{
    public async Task<Cart?> GetByCustomerIdAsync(
        Guid customerId,
        CancellationToken cancellationToken)
    {
        return await context.Carts
            .Include(cart => cart.Items)
            .FirstOrDefaultAsync(
                cart => cart.CustomerId == customerId,
                cancellationToken);
    }

    public async Task AddAsync(
        Cart cart,
        CancellationToken cancellationToken)
    {
        await context.Carts.AddAsync(cart, cancellationToken);
    }
}
