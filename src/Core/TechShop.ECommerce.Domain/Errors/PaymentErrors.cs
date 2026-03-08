namespace TechShop.ECommerce.Domain.Errors;

public static class PaymentErrors
{
    public static Error NotFound(string sessionId) =>
        Error.NotFound(
            "Payment.NotFound",
            $"Payment for session {sessionId} was not found.");
}