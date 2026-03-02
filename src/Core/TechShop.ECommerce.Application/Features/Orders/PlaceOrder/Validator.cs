using TechShop.ECommerce.Application.Features.Orders.Shared;

namespace TechShop.ECommerce.Application.Features.Orders.PlaceOrder;

public sealed class Validator
    : AbstractValidator<Command>
{
    public Validator()
    {
        RuleFor(x => x.ShippingAddress)
            .SetValidator(new AddressDtoValidator());

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}