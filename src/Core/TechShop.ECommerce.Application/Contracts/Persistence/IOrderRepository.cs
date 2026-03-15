namespace TechShop.ECommerce.Application.Contracts.Persistence;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken token = default);
    Task<Order?> GetByIdWithItemsAsync(Guid id, CancellationToken token = default);
    Task<List<Order>> GetPendingOrdersCreatedBeforeAsync(
        DateTimeOffset cutoffUtc,
        CancellationToken cancellationToken);
}
