using TechShop.ECommerce.Application.Common.Errors;
using TechShop.ECommerce.Application.Common.Results;
using TechShop.ECommerce.Application.Common.Telemetry;
using TechShop.ECommerce.Application.Contracts.Identity;
using TechShop.ECommerce.Application.Contracts.PaymentGateway;
using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Domain.Entities.Carts;
using TechShop.ECommerce.Domain.Entities.Catalogs;
using TechShop.ECommerce.Domain.Entities.Orders;
using TechShop.ECommerce.Domain.Entities.Payments;
using TechShop.ECommerce.Domain.Errors;
using TechShop.ECommerce.Domain.ValueObjects;

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
    ILogger<PlaceOrderCommandHandler> logger)
    : IRequestHandler<PlaceOrderCommand, Result<PlaceOrderResponse>>
{
    public async Task<Result<PlaceOrderResponse>> Handle(
        PlaceOrderCommand command,
        CancellationToken token)
    {
        using var activity = TelemetryConfig.ActivitySource
            .StartActivity("orders.place-order");

        activity?.SetTag("operation", "place-order");

        var currentUserResult = GetCurrentUser();
        if (currentUserResult.IsFailure)
        {
            activity?.SetTag("result", "unauthorized");
            return currentUserResult.Error;
        }

        var (customerId, customerEmail) = currentUserResult.Value;

        activity?.SetTag("customer.id", customerId);
        activity?.SetTag("customer.email", customerEmail);

        Result<Cart> cartResult;
        using (TelemetryConfig.ActivitySource.StartActivity("orders.load-cart"))
        {
            cartResult = await GetCartAsync(customerId, token);
        }

        if (cartResult.IsFailure)
        {
            activity?.SetTag("result", "cart-invalid");
            return cartResult.Error;
        }

        var cart = cartResult.Value;
        activity?.SetTag("cart.items.count", cart.Items.Count);

        Result<IReadOnlyDictionary<Guid, Product>> productsResult;
        using (TelemetryConfig.ActivitySource.StartActivity("orders.validate-products"))
        {
            productsResult = await GetValidatedProductsAsync(cart, token);
        }

        if (productsResult.IsFailure)
        {
            activity?.SetTag("result", "products-invalid");
            return productsResult.Error;
        }

        var products = productsResult.Value;

        Order order;
        using (TelemetryConfig.ActivitySource.StartActivity("orders.create-order"))
        {
            order = CreateOrder(
                customerId,
                customerEmail,
                command,
                cart,
                products);
        }

        activity?.SetTag("order.id", order.Id);
        activity?.SetTag("order.total_amount", (double)order.TotalAmount);

        CheckoutSessionResult checkoutSession;
        using (TelemetryConfig.ActivitySource.StartActivity("orders.create-checkout-session"))
        {
            checkoutSession = await paymentService.CreateCheckoutSessionAsync(
                order.Id,
                order.TotalAmount,
                token);
        }

        activity?.SetTag("payment.session_id", checkoutSession.SessionId);
        activity?.SetTag("payment.currency", checkoutSession.Currency);

        Payment payment;
        using (TelemetryConfig.ActivitySource.StartActivity("orders.create-payment"))
        {
            payment = CreatePayment(order, checkoutSession);
        }

        using (TelemetryConfig.ActivitySource.StartActivity("orders.persist"))
        {
            await PersistOrderAsync(order, payment, cart, token);
        }

        TechShopMetrics.OrdersCreated.Add(1);

        activity?.SetTag("result", "success");

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
            .Select(cartItem => cartItem.ProductId)
            .Distinct()
            .ToList();

        var products = await productRepository.GetByIdAsync(productIds, token);
        var productDictionary = products.ToDictionary(product => product.Id);

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

    private async Task PersistOrderAsync(
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
