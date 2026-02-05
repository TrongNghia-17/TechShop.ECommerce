namespace TechShop.ECommerce.Api.Extensions;

public static class HttpCachingExtensions
{
    public static bool IsNotModified(this HttpRequest request, string etag)
    {
        if (!request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var inm))
            return false;

        return inm.ToString().Contains(etag, StringComparison.Ordinal);
    }

    public static void ApplyCacheHeaders(this HttpResponse response, string etag, int maxAgeSeconds = 60)
    {
        response.Headers[HeaderNames.ETag] = etag;
        response.Headers[HeaderNames.CacheControl] = $"private, max-age={maxAgeSeconds}";
    }

    public static string BuildWeakEtag(Guid id, DateTimeOffset lastModifiedUtc)
        => $"W/\"product-{id}-{lastModifiedUtc.ToUniversalTime().Ticks}\"";
}
