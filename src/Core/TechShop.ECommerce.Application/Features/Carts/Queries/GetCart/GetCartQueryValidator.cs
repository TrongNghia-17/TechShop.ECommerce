namespace TechShop.ECommerce.Application.Features.Carts.Queries.GetCart;

public sealed class GetCartQueryValidator
    : AbstractValidator<GetCartQuery>
{
    public GetCartQueryValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("CustomerId is required.");
    }
}