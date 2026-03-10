namespace TechShop.ECommerce.Application.Contracts.Persistence;

public interface IPaymentRepository
{
    Task AddAsync(Payment payment, CancellationToken token);

    Task<Payment?> GetBySessionIdAsync(
        string sessionId,
        CancellationToken cancellationToken);

    Task<List<Payment>> GetPendingByOrderIdsAsync(
        IReadOnlyCollection<Guid> orderIds,
        CancellationToken cancellationToken);
}
