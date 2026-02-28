namespace TechShop.ECommerce.Application.Common.Caching;

public static class CacheKeys
{
    private const string Prefix = "techshop";

    public static class Products
    {
        public const string VersionKey = $"{Prefix}:products:version";

        public static string All()
            => $"{Prefix}:products:all";

        public static string ById(Guid id)
            => $"{Prefix}:products:{id}";

        public static string Paged(ProductQueryFilter filter, int version)
        {
            var builder = new StringBuilder();
            builder.Append($"v{version}:products:paged:");
            builder.Append($"p:{filter.PageNumber}:");
            builder.Append($"s:{filter.PageSize}:");
            builder.Append($"c:{filter.CategoryId ?? Guid.Empty}:");
            builder.Append($"sort:{filter.SortBy ?? "none"}:");
            builder.Append($"q:{filter.Search ?? "none"}");

            return $"{Prefix}:{builder}";
        }

        public static string PagedBase(ProductQueryFilter filter)
        {
            var builder = new StringBuilder();
            builder.Append("products:paged:");
            builder.Append($"p:{filter.PageNumber}:");
            builder.Append($"s:{filter.PageSize}:");
            builder.Append($"c:{filter.CategoryId ?? Guid.Empty}:");
            builder.Append($"sort:{filter.SortBy ?? "none"}:");
            builder.Append($"q:{filter.Search ?? "none"}");
            return $"{Prefix}:{builder}";
        }
    }
}