namespace TechShop.ECommerce.Domain.Errors;

public static class OrderErrors
{
    public static DomainErrors EmptyCart =>
        DomainErrors.Validation(
            "Order.EmptyCart",
            "Cannot place an order with an empty cart.");

    public static DomainErrors NotFound(Guid orderId) =>
        DomainErrors.NotFound(
            "Order.NotFound",
            $"Order {orderId} was not found.");
}
