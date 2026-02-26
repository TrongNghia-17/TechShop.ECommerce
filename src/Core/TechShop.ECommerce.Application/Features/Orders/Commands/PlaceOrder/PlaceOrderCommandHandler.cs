namespace TechShop.ECommerce.Application.Features.Orders.Commands.PlaceOrder;

public class PlaceOrderCommandHandler(
    IPublisher publisher,
    ICurrentUserService currentUserService,
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
        var customerId = currentUserService.UserId;

        var cart = await cartRepository.GetByCustomerIdAsync(customerId, token)
                   ?? throw new NotFoundException(nameof(Cart), customerId);

        cart.EnsureNotEmpty();

        var address = mapper.Map<Address>(command.ShippingAddress);

        var order = Order.Create(customerId, address, command.Notes);

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
            new OrderPlacedNotification(order.Id, customerId),
            token);

        logger.LogInformation("Order {OrderId} created successfully", order.Id);

        return order.Id;
    }
}
