using TechShop.ECommerce.Persistence.DatabaseContext;

namespace TechShop.ECommerce.Persistence.Repositories;

public sealed class OrderRepository(TechShopDbContext context) : IOrderRepository
{
    public async Task AddAsync(Order order, CancellationToken token = default)
    {
        await context.Orders.AddAsync(order, token);
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return await context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id, token);
    }
}
