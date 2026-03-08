namespace TechShop.ECommerce.Domain.Errors;

public static class ProductErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Product.NotFound", $"Product {id} was not found.");

    public static Error InsufficientStock(Guid id) =>
        Error.Conflict(
            "Product.InsufficientStock",
            $"Product {id} does not have enough stock.");
}
