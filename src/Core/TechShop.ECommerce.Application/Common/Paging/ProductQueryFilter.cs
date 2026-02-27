namespace TechShop.ECommerce.Application.Common.Paging;

public sealed class ProductQueryFilter
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public Guid? CategoryId { get; init; }
    public string? SortBy { get; init; }
    public string? Search { get; init; }
}