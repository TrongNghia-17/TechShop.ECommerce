namespace TechShop.ECommerce.Infrastructure.Jobs.Payments;

public interface IHangfirePaymentJobExecutor
{
    Task HandleRefundRequired(Guid paymentId, Guid orderId);
}