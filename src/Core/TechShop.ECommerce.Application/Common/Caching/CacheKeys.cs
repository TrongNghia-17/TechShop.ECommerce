namespace TechShop.ECommerce.Application.Common.Caching;

public static class CacheKeys
{
    private const string Prefix = "techshop";

    public static class Products
    {
        public const string VersionKey = $"{Prefix}:products:version";

        private static string Base(int version, string key)
            => $"{Prefix}:v{version}:{key}";

        public static string All(int version)
            => Base(version, "products:all");

        public static string ById(Guid id, int version)
            => Base(version, $"products:{id}");

        public static string Paged(ProductQueryFilter filter, int version)
        {
            var builder = new StringBuilder();
            builder.Append("products:paged:");
            builder.Append($"p:{filter.PageNumber}:");
            builder.Append($"s:{filter.PageSize}:");
            builder.Append($"c:{filter.CategoryId ?? Guid.Empty}:");
            builder.Append($"sort:{filter.SortBy ?? "none"}:");
            builder.Append($"q:{filter.Search ?? "none"}");

            return Base(version, builder.ToString());
        }
    }
}