namespace TechShop.ECommerce.Domain.Errors;

public static class ProductErrors
{
    public static Error NotFound(Guid productId) =>
        Error.NotFound("Product.NotFound", $"Product {productId} was not found.");

    public static Error InsufficientStock(Guid productId) =>
        Error.Conflict(
            "Product.InsufficientStock",
            $"Product {productId} does not have enough stock.");
}
