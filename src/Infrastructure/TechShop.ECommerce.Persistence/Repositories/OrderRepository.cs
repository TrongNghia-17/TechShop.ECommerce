namespace TechShop.ECommerce.Persistence.Repositories;

public class OrderRepository(TechShopDatabaseContext context) : IOrderRepository
{
    public async Task AddAsync(Order order, CancellationToken token = default)
    {
        await context.Orders.AddAsync(order, token);
    }
}
