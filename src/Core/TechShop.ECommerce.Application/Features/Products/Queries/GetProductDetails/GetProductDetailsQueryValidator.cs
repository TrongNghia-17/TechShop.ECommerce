namespace TechShop.ECommerce.Application.Features.Products.Queries.GetProductDetails;

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
