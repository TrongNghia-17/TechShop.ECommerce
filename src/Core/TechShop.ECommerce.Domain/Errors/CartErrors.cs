namespace TechShop.ECommerce.Domain.Errors;

public static class CartErrors
{
    public static DomainErrors InvalidQuantity =>
        DomainErrors.Validation(
            "Cart.InvalidQuantity",
            "Quantity must be greater than zero.");

    public static DomainErrors NotFound(Guid customerId) =>
        DomainErrors.NotFound(
            "Cart.NotFound",
            $"Cart for customer {customerId} was not found.");
}