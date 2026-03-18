using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Persistence.Context;

namespace TechShop.ECommerce.Persistence.Repositories;

public sealed class PaymentRepository(TechShopDbContext context) : IPaymentRepository
{
    public async Task AddAsync(
        Payment payment,
        CancellationToken cancellationToken)
    {
        await context.Payments.AddAsync(payment, cancellationToken);
    }

    public async Task<Payment?> GetBySessionIdAsync(
        string sessionId,
        CancellationToken cancellationToken)
    {
        return await context.Payments
            .FirstOrDefaultAsync(
                payment => payment.StripeCheckoutSessionId == sessionId,
                cancellationToken);
    }

    public async Task<Payment?> GetByIdAsync(
        Guid paymentId,
        CancellationToken cancellationToken)
    {
        return await context.Payments
            .FirstOrDefaultAsync(
                payment => payment.Id == paymentId,
                cancellationToken);
    }

    public async Task<List<Payment>> GetPendingByOrderIdsAsync(
        IReadOnlyCollection<Guid> orderIds,
        CancellationToken cancellationToken)
    {
        if (orderIds.Count == 0)
            return [];

        return await context.Payments
            .Where(payment =>
                orderIds.Contains(payment.OrderId) &&
                payment.Status == PaymentStatus.Pending)
            .ToListAsync(cancellationToken);
    }
}