namespace TechShop.ECommerce.Application.Features.Products.Commands.BulkUpdatePrice;

public class BulkUpdatePriceCommandValidator : AbstractValidator<BulkUpdatePriceCommand>
{
    public BulkUpdatePriceCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty();

        RuleFor(x => x.PercentageChange)
            .GreaterThanOrEqualTo(-50).WithMessage("Cannot discount more than 50%")
            .LessThanOrEqualTo(100);
    }
}