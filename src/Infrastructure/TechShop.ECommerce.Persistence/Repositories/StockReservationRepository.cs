using TechShop.ECommerce.Domain.Entities.Inventory;

namespace TechShop.ECommerce.Persistence.Repositories;

public class StockReservationRepository(TechShopDatabaseContext context) : IStockReservationRepository
{
    public async Task AddAsync(
        StockReservation reservation,
        CancellationToken token)
    {
        await context.StockReservations.AddAsync(reservation, token);
    }

    public async Task<List<StockReservation>> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken token)
    {
        return await context.StockReservations
            .Where(r => r.OrderId == orderId)
            .ToListAsync(token);
    }

    public async Task<List<StockReservation>> GetExpiredAsync(
        CancellationToken token)
    {
        var now = DateTimeOffset.UtcNow;

        return await context.StockReservations
            .Where(r => r.ExpiresAtUtc <= now)
            .ToListAsync(token);
    }

    public void RemoveRange(List<StockReservation> reservations)
    {
        context.StockReservations.RemoveRange(reservations);
    }
}