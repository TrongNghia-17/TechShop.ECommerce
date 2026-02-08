using System.Text.Json;

namespace TechShop.ECommerce.Application.Common.Cursors;

public static class CursorEncoder
{
    public static string Encode<TCursor>(TCursor cursor)
        => Convert.ToBase64String(
            JsonSerializer.SerializeToUtf8Bytes(cursor));

    public static TCursor Decode<TCursor>(string value)
        => JsonSerializer.Deserialize<TCursor>(
            Convert.FromBase64String(value))!;
}
