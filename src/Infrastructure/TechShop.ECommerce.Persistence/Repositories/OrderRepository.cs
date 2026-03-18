namespace TechShop.ECommerce.Persistence.Repositories;

public sealed class OrderRepository(TechShopDbContext context) : IOrderRepository
{
    public async Task AddAsync(
        Order order,
        CancellationToken cancellationToken)
    {
        await context.Orders.AddAsync(order, cancellationToken);
    }

    public async Task<Order?> GetByIdWithItemsAsync(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        return await context.Orders
            .Include(order => order.OrderItems)
            .FirstOrDefaultAsync(
                order => order.Id == orderId,
                cancellationToken);
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
