namespace TechShop.ECommerce.Application.Features.Products.GetProducts;

public sealed class GetProductsQueryValidator
    : AbstractValidator<GetProductsQuery>
{
    private static readonly string[] AllowedSortColumns = ["name", "price"];

    public GetProductsQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 50);

        RuleFor(x => x.SortBy)
            .Must(BeValidSortBy)
            .When(x => !string.IsNullOrWhiteSpace(x.SortBy))
            .WithMessage("SortBy must be in format '<column>' or '<column> desc'.");
    }

    private static bool BeValidSortBy(string? sortBy)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return true;

        var parts = sortBy.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length is < 1 or > 2)
            return false;

        var column = parts[0].ToLowerInvariant();

        if (!AllowedSortColumns.Contains(column))
            return false;

        if (parts.Length == 2 &&
            !parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase))
            return false;

        return true;
    }
}
