namespace TechShop.ECommerce.Persistence.Repositories;

public sealed class OrderRepository(TechShopDbContext context) : IOrderRepository
{
    public async Task AddAsync(Order order, CancellationToken token = default)
    {
        await context.Orders.AddAsync(order, token);
    }

    public async Task<Order?> GetByIdWithItemsAsync(Guid id, CancellationToken token = default)
    {
        return await context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id, token);
    }

    public async Task<List<Order>> GetPendingOrdersCreatedBeforeAsync(
        DateTimeOffset cutoffUtc,
        CancellationToken cancellationToken)
    {
        return await context.Orders
            .Where(order =>
                order.Status == OrderStatus.PendingPayment &&
                order.OrderDate <= cutoffUtc)
            .ToListAsync(cancellationToken);
    }
}
