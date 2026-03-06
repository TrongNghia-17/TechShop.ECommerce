using TechShop.ECommerce.Persistence.DatabaseContext;

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
            .FirstOrDefaultAsync(p => p.StripePaymentIntentId == sessionId, token);
    }
}