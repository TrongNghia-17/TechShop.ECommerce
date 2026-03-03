namespace TechShop.ECommerce.Persistence.Repositories;

public sealed class PaymentRepository(TechShopDatabaseContext context) : IPaymentRepository
{
    public async Task AddAsync(Payment payment, CancellationToken token)
    {
        await context.Payments.AddAsync(payment, token);
    }

    public async Task<Payment?> GetBySessionIdAsync(string sessionId, CancellationToken token)
    {
        return await context.Payments
            .FirstOrDefaultAsync(p => p.StripePaymentIntentId == sessionId, token);
    }
}