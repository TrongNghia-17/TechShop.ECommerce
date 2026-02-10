namespace TechShop.ECommerce.Application.Features.Products.Commands.BulkPurge;

public class BulkPurgeProductsCommandValidator : AbstractValidator<BulkPurgeProductsCommand>
{
    public BulkPurgeProductsCommandValidator()
    {
        RuleFor(x => x.DaysOld)
            .GreaterThanOrEqualTo(7)
            .WithMessage("Safety guard: Can only purge products deleted more than 7 days ago.");
    }
}
