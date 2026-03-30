namespace TechShop.ECommerce.Domain.Errors;

public static class ProductErrors
{
    public static DomainErrors NotFound(Guid productId) =>
        DomainErrors.NotFound("Product.NotFound", $"Product {productId} was not found.");

    public static DomainErrors InsufficientStock(Guid productId) =>
        DomainErrors.Conflict(
            "Product.InsufficientStock",
            $"Product {productId} does not have enough stock.");
}
