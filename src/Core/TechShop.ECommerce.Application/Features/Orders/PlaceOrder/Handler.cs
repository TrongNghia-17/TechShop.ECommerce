using TechShop.ECommerce.Application.Contracts.PaymentGateway;
using TechShop.ECommerce.Domain.Entities.Payments;

namespace TechShop.ECommerce.Application.Features.Orders.PlaceOrder;

public class Handler(
    ICurrentUserService currentUserService,
    IPaymentService paymentService,
    ICartRepository cartRepository,
    IProductRepository productRepository,
    IOrderRepository orderRepository,
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IAppLogger<Handler> logger)
    : IRequestHandler<Command, Result<Response>>
{
    public async Task<Result<Response>> Handle(
    Command command,
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

            await orderRepository.AddAsync(order, token);
            cart.Clear();

        }, token);

        var session = await paymentService.CreateCheckoutSessionAsync(
            order.Id,
            order.TotalAmount,
            token);

        var payment = Payment.Create(
            order.Id,
            session.SessionId,
            order.TotalAmount,
            session.Currency);

        await paymentRepository.AddAsync(payment, token);
        await unitOfWork.SaveChangesAsync(token);

        logger.LogInformation(
            "Order {OrderId} created successfully for customer {CustomerId}",
            order.Id,
            customerId);

        return new Response(order.Id, session.Url);
    }
}
