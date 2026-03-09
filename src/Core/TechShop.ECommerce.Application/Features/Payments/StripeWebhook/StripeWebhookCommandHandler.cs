using TechShop.ECommerce.Application.Features.Orders.Notifications;

namespace TechShop.ECommerce.Application.Features.Payments.StripeWebhook;

public sealed class StripeWebhookCommandHandler(
    IPaymentRepository paymentRepository,
    IOrderRepository orderRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    IPublisher publisher,
    ILogger<StripeWebhookCommandHandler> logger)
    : IRequestHandler<StripeWebhookCommand, Result>
{
    public async Task<Result> Handle(StripeWebhookCommand command, CancellationToken token)
    {
        if (!IsCheckoutSessionCompleted(command))
            return Result.Success();

        var paymentResult = await GetPaymentAsync(command.SessionId, token);
        if (paymentResult.IsFailure)
            return paymentResult.Error;

        var payment = paymentResult.Value;

        if (payment.Status == PaymentStatus.Succeeded)
        {
            logger.LogInformation(
                "Duplicate webhook ignored for session {SessionId}",
                command.SessionId);

            return Result.Success();
        }

        var orderResult = await GetOrderAsync(command.OrderId, token);
        if (orderResult.IsFailure)
            return orderResult.Error;

        var order = orderResult.Value;

        var stockValidationResult = await ValidateAndDeductStockAsync(order, token);
        if (stockValidationResult.IsFailure)
            return stockValidationResult.Error;

        payment.MarkSucceeded();
        order.Confirm();

        await unitOfWork.SaveChangesAsync(token);

        await PublishOrderPlacedNotificationAsync(order, token);

        logger.LogInformation(
            "Order {OrderId} confirmed and stock deducted",
            order.Id);

        return Result.Success();
    }

    private static bool IsCheckoutSessionCompleted(StripeWebhookCommand command)
    {
        return command.EventType == "checkout.session.completed";
    }

    private async Task<Result<Payment>> GetPaymentAsync(
        string sessionId,
        CancellationToken token)
    {
        var payment = await paymentRepository.GetBySessionIdAsync(sessionId, token);

        if (payment is null)
        {
            logger.LogWarning(
                "Payment not found for session {SessionId}",
                sessionId);

            return PaymentErrors.NotFound(sessionId);
        }

        return payment;
    }

    private async Task<Result<Order>> GetOrderAsync(
       Guid orderId,
       CancellationToken token)
    {
        var order = await orderRepository.GetByIdAsync(orderId, token);

        if (order is null)
        {
            logger.LogError(
                "Order {OrderId} not found while handling Stripe webhook",
                orderId);

            return OrderErrors.NotFound(orderId);
        }

        return order;
    }

    private async Task<Result> ValidateAndDeductStockAsync(
       Order order,
       CancellationToken token)
    {
        foreach (var orderItem in order.OrderItems)
        {
            var product = await productRepository.GetByIdAsync(orderItem.ProductId, token);

            if (product is null)
                return ProductErrors.NotFound(orderItem.ProductId);

            if (!product.HasEnoughStock(orderItem.Quantity))
                return ProductErrors.InsufficientStock(orderItem.ProductId);

            product.RemoveStock(orderItem.Quantity);
        }

        return Result.Success();
    }

    private async Task PublishOrderPlacedNotificationAsync(
        Order order,
        CancellationToken token)
    {
        await publisher.Publish(
            new OrderConfirmedNotification(order.Id, order.CustomerId),
            token);
    }
}