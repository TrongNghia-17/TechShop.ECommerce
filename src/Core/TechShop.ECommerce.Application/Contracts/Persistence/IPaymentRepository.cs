using TechShop.ECommerce.Domain.Entities.Payments;

namespace TechShop.ECommerce.Application.Contracts.Persistence;

public interface IPaymentRepository
{
    Task AddAsync(Payment payment, CancellationToken token);
    Task<Payment?> GetBySessionIdAsync(string sessionId, CancellationToken token);
}
