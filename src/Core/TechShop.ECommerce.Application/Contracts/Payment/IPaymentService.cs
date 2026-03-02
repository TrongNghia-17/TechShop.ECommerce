namespace TechShop.ECommerce.Application.Contracts.Payment;

public interface IPaymentService
{
    Task<string> CreatePaymentIntent(decimal amount);
}
