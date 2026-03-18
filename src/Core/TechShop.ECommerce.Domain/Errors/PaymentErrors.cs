namespace TechShop.ECommerce.Domain.Errors;

public static class PaymentErrors
{
    public static DomainErrors NotFound(string sessionId) =>
        DomainErrors.NotFound(
            "Payment.NotFound",
            $"Payment for session {sessionId} was not found.");
}