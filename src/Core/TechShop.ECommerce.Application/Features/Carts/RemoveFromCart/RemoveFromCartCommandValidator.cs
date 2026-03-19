namespace TechShop.ECommerce.Application.Features.Carts.RemoveFromCart;

public sealed class RemoveFromCartCommandValidator
    : AbstractValidator<RemoveFromCartCommand>
{
    public RemoveFromCartCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("ProductId is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.")
            .LessThanOrEqualTo(100)
            .WithMessage("Quantity must be less than or equal to 100.");
    }
}