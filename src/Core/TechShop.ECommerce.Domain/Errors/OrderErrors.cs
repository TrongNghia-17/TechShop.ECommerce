namespace TechShop.ECommerce.Domain.Errors;

public static class OrderErrors
{
    public static Error EmptyCart =>
        Error.Validation(
            "Order.EmptyCart",
            "Cannot place an order with an empty cart.");

    public static Error NotFound(Guid orderId) =>
        Error.NotFound(
            "Order.NotFound",
            $"Order {orderId} was not found.");
}
