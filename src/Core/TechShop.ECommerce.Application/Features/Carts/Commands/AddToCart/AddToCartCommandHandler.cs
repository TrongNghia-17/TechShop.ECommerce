using TechShop.ECommerce.Domain.Entities.Carts;

namespace TechShop.ECommerce.Application.Features.Carts.Commands.AddToCart;

public sealed class AddToCartCommandHandler(
    ICartRepository cartRepository,
    IProductRepository productRepository,
    ICurrentUserService currentUserService,
    IUnitOfWork unitOfWork
) : IRequestHandler<AddToCartCommand, Result<AddToCartResult>>
{
    public async Task<Result<AddToCartResult>> Handle(
        AddToCartCommand command,
        CancellationToken token)
    {
        var customerId = currentUserService.UserId;

        if (customerId == Guid.Empty)
            return IdentityErrors.Unauthorized;

        var product = await productRepository
            .GetByIdAsync(command.ProductId, token);

        if (product is null)
            return ProductErrors.NotFound(command.ProductId);

        var cart = await cartRepository
            .GetByCustomerIdAsync(customerId, token);

        if (cart is null)
        {
            cart = Cart.Create(customerId);
            await cartRepository.AddAsync(cart, token);
        }

        var existingCartItem = cart.Items
            .FirstOrDefault(item => item.ProductId == command.ProductId);

        var requestedQuantity = (existingCartItem?.Quantity ?? 0) + command.Quantity;

        if (!product.HasEnoughStock(requestedQuantity))
            return ProductErrors.InsufficientStock(command.ProductId);

        cart.AddItem(command.ProductId, product.Price, command.Quantity);

        await unitOfWork.SaveChangesAsync(token);

        return new AddToCartResult(cart.Id, cart.GetTotal());
    }
}
