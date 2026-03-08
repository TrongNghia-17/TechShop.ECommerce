namespace TechShop.ECommerce.Application.Features.Orders.PlaceOrder;

public class PlaceOrderCommandHandler(
    ICurrentUserService currentUserService,
    IPaymentService paymentService,
    ICartRepository cartRepository,
    IProductRepository productRepository,
    IOrderRepository orderRepository,
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IAppLogger<PlaceOrderCommandHandler> logger)
    : IRequestHandler<PlaceOrderCommand, Result<PlaceOrderResponse>>
{
    public async Task<Result<PlaceOrderResponse>> Handle(
        PlaceOrderCommand command,
        CancellationToken token)
    {
        var currentUserResult = GetCurrentUser();
        if (currentUserResult.IsFailure)
            return currentUserResult.Error;

        var (customerId, customerEmail) = currentUserResult.Value;

        var cartResult = await GetCartAsync(customerId, token);
        if (cartResult.IsFailure)
            return cartResult.Error;

        var cart = cartResult.Value;

        var productsResult = await GetValidatedProductsAsync(cart, token);
        if (productsResult.IsFailure)
            return productsResult.Error;

        var products = productsResult.Value;

        var order = CreateOrder(
            customerId,
            customerEmail,
            command,
            cart,
            products);

        var checkoutSession = await paymentService.CreateCheckoutSessionAsync(
            order.Id,
            order.TotalAmount,
            token);

        var payment = CreatePayment(order, checkoutSession);

        await PersistAsync(order, payment, cart, token);

        logger.LogInformation(
            "Order {OrderId} created successfully for customer {CustomerId}",
            order.Id,
            customerId);

        return new PlaceOrderResponse(order.Id, checkoutSession.Url);
    }

    private Result<(Guid CustomerId, string CustomerEmail)> GetCurrentUser()
    {
        var customerId = currentUserService.UserId;
        var customerEmail = currentUserService.Email;

        if (customerId == Guid.Empty)
            return IdentityErrors.Unauthorized;

        if (string.IsNullOrWhiteSpace(customerEmail))
            return IdentityErrors.EmailNotFound;

        return (customerId, customerEmail);
    }

    private async Task<Result<Cart>> GetCartAsync(
        Guid customerId,
        CancellationToken token)
    {
        var cart = await cartRepository.GetByCustomerIdAsync(customerId, token);

        if (cart is null)
            return CartErrors.NotFound(customerId);

        if (cart.Items.Count == 0)
            return OrderErrors.EmptyCart;

        return cart;
    }

    private async Task<Result<IReadOnlyDictionary<Guid, Product>>> GetValidatedProductsAsync(
       Cart cart,
       CancellationToken token)
    {
        var productIds = cart.Items
            .Select(x => x.ProductId)
            .Distinct()
            .ToList();

        var products = await productRepository.GetByIdAsync(productIds, token);
        var productDictionary = products.ToDictionary(x => x.Id);

        foreach (var item in cart.Items)
        {
            if (!productDictionary.TryGetValue(item.ProductId, out var product))
                return ProductErrors.NotFound(item.ProductId);

            if (!product.HasEnoughStock(item.Quantity))
                return ProductErrors.InsufficientStock(item.ProductId);
        }

        return productDictionary;
    }

    private Order CreateOrder(
        Guid customerId,
        string customerEmail,
        PlaceOrderCommand command,
        Cart cart,
        IReadOnlyDictionary<Guid, Product> products)
    {
        var shippingAddress = mapper.Map<Address>(command.ShippingAddress);

        var order = Order.Create(
            customerId,
            customerEmail,
            shippingAddress,
            command.Notes);

        foreach (var cartItem in cart.Items)
        {
            var product = products[cartItem.ProductId];

            order.AddItem(
                product.Id,
                product.Name,
                product.Price,
                cartItem.Quantity);
        }

        return order;
    }

    private static Payment CreatePayment(
        Order order,
        CheckoutSessionResult checkoutSession)
    {
        return Payment.Create(
            order.Id,
            checkoutSession.SessionId,
            order.TotalAmount,
            checkoutSession.Currency);
    }

    private async Task PersistAsync(
        Order order,
        Payment payment,
        Cart cart,
        CancellationToken token)
    {
        await orderRepository.AddAsync(order, token);
        await paymentRepository.AddAsync(payment, token);

        cart.Clear();

        await unitOfWork.SaveChangesAsync(token);
    }
}
