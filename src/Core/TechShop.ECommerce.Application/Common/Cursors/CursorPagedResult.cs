namespace TechShop.ECommerce.Application.Common.Cursors;

public sealed class CursorPagedResult<T>
{
    public required IReadOnlyList<T> Items { get; init; }
    public required int Limit { get; init; }
    public string? NextCursor { get; init; }
    public bool HasNext => NextCursor is not null;
}
