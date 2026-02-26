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
    : IRequestHandler<PlaceOrderCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
    PlaceOrderCommand command,
    CancellationToken token)
    {
        var customerId = currentUserService.UserId;

        var cart = await cartRepository
            .GetByCustomerIdAsync(customerId, token);

        if (cart is null)
            return DomainErrors.Cart.NotFound(customerId);

        if (cart.Items.Count == 0)
            return DomainErrors.Order.EmptyCart;

        var address = mapper.Map<Address>(command.ShippingAddress);
        var order = Order.Create(customerId, address, command.Notes);

        var productIds = cart.Items
            .Select(i => i.ProductId)
            .Distinct()
            .ToList();

        var products = await productRepository
            .GetByIdAsync(productIds, token);

        var productDict = products
            .ToDictionary(p => p.Id);

        foreach (var item in cart.Items)
        {
            if (!productDict.TryGetValue(item.ProductId, out var product))
                return DomainErrors.Product.NotFound(item.ProductId);

            if (!product.HasEnoughStock(item.Quantity))
                return DomainErrors.Product.InsufficientStock(item.ProductId);
        }

        await unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            foreach (var item in cart.Items)
            {
                var product = productDict[item.ProductId];

                product.RemoveStock(item.Quantity);
                order.AddItem(product.Id, product.Price, item.Quantity);
            }

            order.Confirm();

            await orderRepository.AddAsync(order, token);

            cart.Clear();

        }, token);

        await publisher.Publish(
            new OrderPlacedNotification(order.Id, customerId),
            token);

        logger.LogInformation(
            "Order {OrderId} created successfully",
            order.Id);

        return order.Id;
    }
}
