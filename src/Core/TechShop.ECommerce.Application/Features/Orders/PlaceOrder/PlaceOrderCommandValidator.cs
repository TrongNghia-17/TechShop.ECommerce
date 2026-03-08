using TechShop.ECommerce.Application.Features.Orders.Shared;

namespace TechShop.ECommerce.Application.Features.Orders.PlaceOrder;

public sealed class PlaceOrderCommandValidator
    : AbstractValidator<PlaceOrderCommand>
{
    public PlaceOrderCommandValidator()
    {
        RuleFor(x => x.ShippingAddress)
            .SetValidator(new AddressDtoValidator());

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}