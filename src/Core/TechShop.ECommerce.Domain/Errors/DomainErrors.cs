namespace TechShop.ECommerce.Domain.Errors;

public static class DomainErrors
{
    public static class Product
    {
        public static Error NotFound(Guid id) =>
            Error.NotFound("Product.NotFound", $"Product {id} was not found");

        public static Error InsufficientStock(Guid id) =>
            Error.Conflict(
                "Product.InsufficientStock",
                $"Product {id} does not have enough stock");
    }

    public static class Cart
    {
        public static Error InvalidQuantity =>
            Error.Validation("Cart.InvalidQuantity", "Quantity must be greater than 0");

        public static Error NotFound(Guid customerId) =>
            Error.NotFound("Cart.NotFound", $"Cart for customer {customerId} was not found");
    }

    public static class Order
    {
        public static Error EmptyCart =>
            Error.Validation(
                "Order.EmptyCart",
                "Cannot place order with an empty cart.");
    }


    public static class Identity
    {
        public static Error InvalidCredentials =>
            Error.Validation(
                "Identity.InvalidCredentials",
                "Invalid email or password");

        public static Error RegisterFailed(string message) =>
            Error.Validation(
                "Identity.RegisterFailed",
                message);
    }
}
