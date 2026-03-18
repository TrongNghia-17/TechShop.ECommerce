namespace TechShop.ECommerce.Domain.Errors;

public static class CartErrors
{
    public static Error InvalidQuantity =>
        Error.Validation(
            "Cart.InvalidQuantity",
            "Quantity must be greater than zero.");

    public static Error NotFound(Guid customerId) =>
        Error.NotFound(
            "Cart.NotFound",
            $"Cart for customer {customerId} was not found.");
}