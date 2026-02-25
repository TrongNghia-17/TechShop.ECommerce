namespace TechShop.ECommerce.Application.Features.Orders.Commands.PlaceOrder;

public class PlaceOrderCommandHandler(
    IPublisher publisher,
    ICartRepository cartRepository,
    IProductRepository productRepository,
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IAppLogger<PlaceOrderCommandHandler> logger)
    : IRequestHandler<PlaceOrderCommand, Guid>
{
    public async Task<Guid> Handle(PlaceOrderCommand command, CancellationToken token)
    {
        var cart = await cartRepository.GetByCustomerIdAsync(command.CustomerId, token)
                   ?? throw new NotFoundException(nameof(Cart), command.CustomerId);

        cart.EnsureNotEmpty();

        var address = mapper.Map<Address>(command.ShippingAddress);

        var order = Order.Create(command.CustomerId, address, command.Notes);

        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            foreach (var item in cart.Items)
            {
                var product = await productRepository.GetByIdAsync(item.ProductId)
                              ?? throw new NotFoundException(nameof(Product), item.ProductId);

                product.RemoveStock(item.Quantity);

                order.AddItem(product.Id, product.Price, item.Quantity);
            }

            order.Confirm();

            await orderRepository.AddAsync(order);

            cart.Clear();

        }, token);

        await publisher.Publish(
            new OrderPlacedNotification(order.Id, command.CustomerId),
            token);

        logger.LogInformation("Order {OrderId} created successfully", order.Id);

        return order.Id;
    }
}
