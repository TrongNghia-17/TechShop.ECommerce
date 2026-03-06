using TechShop.ECommerce.Persistence.DatabaseContext;

namespace TechShop.ECommerce.Persistence.Repositories;

public sealed class CartRepository(TechShopDbContext context)
    : ICartRepository
{
    public async Task<Cart?> GetByCustomerIdAsync(
        Guid customerId,
        CancellationToken token)
    {
        return await context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken: token);
    }

    public async Task AddAsync(
        Cart cart,
        CancellationToken token)
    {
        await context.Carts.AddAsync(cart, token);
    }
}
