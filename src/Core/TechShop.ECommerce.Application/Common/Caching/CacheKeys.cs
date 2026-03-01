namespace TechShop.ECommerce.Application.Common.Caching;

public static class CacheKeys
{
    private const string Prefix = "techshop";

    public static class Products
    {
        public static string ById(Guid id)
            => $"{Prefix}:products:{id}";
    }
}