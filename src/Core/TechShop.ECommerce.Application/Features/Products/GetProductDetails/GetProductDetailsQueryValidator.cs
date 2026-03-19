namespace TechShop.ECommerce.Application.Features.Products.GetProductDetails;

public sealed class GetProductDetailsQueryValidator
    : AbstractValidator<GetProductDetailsQuery>
{
    public GetProductDetailsQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product Id must not be empty.");
    }
}
