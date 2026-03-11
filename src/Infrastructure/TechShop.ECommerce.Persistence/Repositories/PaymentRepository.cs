namespace TechShop.ECommerce.Persistence.Repositories;

public sealed class PaymentRepository(TechShopDbContext context) : IPaymentRepository
{
    public async Task AddAsync(Payment payment, CancellationToken token)
    {
        await context.Payments.AddAsync(payment, token);
    }

    public async Task<Payment?> GetBySessionIdAsync(string sessionId, CancellationToken token)
    {
        return await context.Payments
            .FirstOrDefaultAsync(p => p.StripeCheckoutSessionId == sessionId, token);
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