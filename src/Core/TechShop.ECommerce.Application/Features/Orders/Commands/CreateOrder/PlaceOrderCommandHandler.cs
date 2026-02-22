namespace TechShop.ECommerce.Application.Features.Orders.Commands.CreateOrder;

public class PlaceOrderCommandHandler(
    ICartRepository cartRepository,
    IProductRepository productRepository,
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork,
    IAppLogger<PlaceOrderCommandHandler> logger)
    : IRequestHandler<PlaceOrderCommand, Guid>
{
    public async Task<Guid> Handle(PlaceOrderCommand request, CancellationToken token)
    {
        Guid orderId = Guid.Empty;

        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var cart = await cartRepository.GetByCustomerIdAsync(request.CustomerId, token)
                      ?? throw new NotFoundException(nameof(Cart), request.CustomerId);

            if (cart.Items is null || cart.Items.Count == 0)
                throw new BadRequestException("Cart is empty.");

            var address = new Address(
                request.ShippingAddress.Street,
                request.ShippingAddress.City,
                request.ShippingAddress.PostalCode,
                request.ShippingAddress.Country
            );

            var order = Order.Create(request.CustomerId, address, request.Notes);

            foreach (var item in cart.Items)
            {
                var product = await productRepository.GetByIdAsync(item.ProductId)
                    ?? throw new NotFoundException(nameof(Product), item.ProductId);

                product.RemoveStock(item.Quantity);

                order.AddItem(product.Id, product.Price, item.Quantity);
            }

            await orderRepository.AddAsync(order);

            cart.Clear();

            orderId = order.Id;

        }, token);

        logger.LogInformation("Order {OrderId} created successfully", orderId);
        return orderId;
    }
}
