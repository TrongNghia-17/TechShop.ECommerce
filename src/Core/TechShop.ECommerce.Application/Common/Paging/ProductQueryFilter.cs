namespace TechShop.ECommerce.Application.Common.Paging;

public sealed class ProductQueryFilter
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}