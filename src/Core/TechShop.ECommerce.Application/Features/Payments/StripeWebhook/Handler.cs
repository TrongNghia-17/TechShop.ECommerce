using TechShop.ECommerce.Application.Features.Orders.PlaceOrder;
using TechShop.ECommerce.Domain.Entities.Payments;

namespace TechShop.ECommerce.Application.Features.Payments.StripeWebhook;

public sealed class Handler(
    IPaymentRepository paymentRepository,
    IOrderRepository orderRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    IPublisher publisher,
    ILogger<Handler> logger)
    : IRequestHandler<Command, Result>
{
    public async Task<Result> Handle(Command command, CancellationToken token)
    {
        if (command.EventType != "checkout.session.completed")
            return Result.Success();

        var payment = await paymentRepository
            .GetBySessionIdAsync(command.SessionId, token);

        if (payment is null)
        {
            logger.LogWarning(
                "Payment not found for session {SessionId}",
                command.SessionId);

            return Result.Failure(
                DomainErrors.Payment.NotFound(command.SessionId));
        }

        if (payment.Status == PaymentStatus.Succeeded)
        {
            logger.LogInformation(
                "Duplicate webhook ignored for session {SessionId}",
                command.SessionId);

            return Result.Success();
        }

        payment.MarkSucceeded();

        var order = await orderRepository
            .GetByIdAsync(command.OrderId, token);

        if (order is null)
        {
            logger.LogError(
                "Order {OrderId} not found while handling Stripe webhook",
                command.OrderId);

            return Result.Failure(
                DomainErrors.Order.NotFound(command.OrderId));
        }

        foreach (var item in order.OrderItems)
        {
            var product = await productRepository
                .GetByIdAsync(item.ProductId, token);

            if (!product!.HasEnoughStock(item.Quantity))
                return DomainErrors.Product.InsufficientStock(item.ProductId);

            product.RemoveStock(item.Quantity);
        }

        order.Confirm();

        await unitOfWork.SaveChangesAsync(token);

        await publisher.Publish(
            new OrderPlacedNotification(order.Id, order.CustomerId),
            token);

        logger.LogInformation(
            "Order {OrderId} confirmed and stock deducted",
            order.Id);

        return Result.Success();
    }
}