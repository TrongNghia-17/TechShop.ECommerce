namespace TechShop.ECommerce.Application.Contracts.Persistence;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken token = default);
    Task<Order?> GetByIdAsync(Guid id, CancellationToken token);
}
