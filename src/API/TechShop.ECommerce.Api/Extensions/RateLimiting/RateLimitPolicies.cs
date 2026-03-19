namespace TechShop.ECommerce.Api.Extensions.RateLimiting;

public static class RateLimitPolicies
{
    public const string AuthFixed = "AuthFixed";
    public const string ProductsReadSliding = "ProductsReadSliding";
    public const string ProductsManagementFixed = "ProductsManagementFixed";
    public const string FileUploadFixed = "FileUploadFixed";
    public const string CartFixed = "CartFixed";
    public const string OrdersFixed = "OrdersFixed";
    public const string WebhookFixed = "WebhookFixed";
}