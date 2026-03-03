using TechShop.ECommerce.Domain.Entities.Inventory;

namespace TechShop.ECommerce.Application.Contracts.Persistence;

public interface IStockReservationRepository
{
    Task AddAsync(StockReservation reservation, CancellationToken token);
    Task<List<StockReservation>> GetByOrderIdAsync(Guid orderId, CancellationToken token);
    Task<List<StockReservation>> GetExpiredAsync(CancellationToken token);
    void RemoveRange(List<StockReservation> reservations);
}
