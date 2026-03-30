using TechShop.ECommerce.Domain.Entities.Orders;

namespace TechShop.ECommerce.Application.Contracts.Persistence;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);
    Task<Order?> GetByIdWithItemsAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Order>> GetPendingOrdersCreatedBeforeAsync(
        DateTimeOffset cutoffUtc,
        CancellationToken cancellationToken);
}
