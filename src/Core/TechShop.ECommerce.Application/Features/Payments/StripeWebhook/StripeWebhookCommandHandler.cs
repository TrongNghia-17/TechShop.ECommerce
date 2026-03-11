using TechShop.ECommerce.Application.Contracts.Jobs;
using TechShop.ECommerce.Application.Features.Orders.Notifications;

namespace TechShop.ECommerce.Application.Features.Payments.StripeWebhook;

public sealed class StripeWebhookCommandHandler(
    IPaymentRepository paymentRepository,
    IOrderRepository orderRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    IPaymentJobs paymentJobs,
    IPublisher publisher,
    ILogger<StripeWebhookCommandHandler> logger)
    : IRequestHandler<StripeWebhookCommand, Result>
{
    public async Task<Result> Handle(
        StripeWebhookCommand command,
        CancellationToken cancellationToken)
    {
        if (!IsCheckoutSessionCompleted(command))
            return Result.Success();

        var paymentResult = await GetPaymentAsync(command.SessionId, cancellationToken);
        if (paymentResult.IsFailure)
            return paymentResult.Error;

        var payment = paymentResult.Value;

        if (IsAlreadyProcessed(payment, command.SessionId))
            return Result.Success();

        var orderResult = await GetOrderAsync(command.OrderId, cancellationToken);
        if (orderResult.IsFailure)
            return orderResult.Error;

        var order = orderResult.Value;

        payment.MarkSucceeded();

        var stockProcessingResult = await TryValidateAndDeductStockAsync(
            order,
            payment,
            cancellationToken);

        if (stockProcessingResult == StockProcessingResult.CancelledAfterPayment)
            return Result.Success();

        order.Confirm();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await PublishOrderPlacedNotificationAsync(order, cancellationToken);

        logger.LogInformation(
            "Order {OrderId} confirmed and stock deducted",
            order.Id);

        return Result.Success();
    }

    private static bool IsCheckoutSessionCompleted(StripeWebhookCommand command)
    {
        return command.EventType == "checkout.session.completed";
    }

    private bool IsAlreadyProcessed(
        Payment payment,
        string sessionId)
    {
        if (payment.Status is PaymentStatus.Succeeded
            or PaymentStatus.RefundPending
            or PaymentStatus.Refunded)
        {
            logger.LogInformation(
                "Duplicate or already processed webhook ignored for session {SessionId}",
                sessionId);

            return true;
        }

        return false;
    }

    private async Task<Result<Payment>> GetPaymentAsync(
        string sessionId,
        CancellationToken cancellationToken)
    {
        var payment = await paymentRepository.GetBySessionIdAsync(sessionId, cancellationToken);

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
       CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(orderId, cancellationToken);

        if (order is null)
        {
            logger.LogError(
                "Order {OrderId} not found while handling Stripe webhook",
                orderId);

            return OrderErrors.NotFound(orderId);
        }

        return order;
    }

    private async Task<StockProcessingResult> TryValidateAndDeductStockAsync(
       Order order,
       Payment payment,
       CancellationToken cancellationToken)
    {
        foreach (var orderItem in order.OrderItems)
        {
            var product = await productRepository.GetByIdAsync(
                orderItem.ProductId,
                cancellationToken);

            if (product is null)
                throw new DomainException($"Product {orderItem.ProductId} was not found.");

            if (!product.HasEnoughStock(orderItem.Quantity))
            {
                await HandleOutOfStockAfterSuccessfulPaymentAsync(
                    order,
                    payment,
                    cancellationToken);

                return StockProcessingResult.CancelledAfterPayment;
            }

            product.RemoveStock(orderItem.Quantity);
        }

        return StockProcessingResult.ReadyToConfirm;
    }

    private enum StockProcessingResult
    {
        ReadyToConfirm = 0,
        CancelledAfterPayment = 1
    }

    private async Task<Result> HandleOutOfStockAfterSuccessfulPaymentAsync(
        Order order,
        Payment payment,
        CancellationToken cancellationToken)
    {
        order.Cancel("Cancelled after successful payment because product is out of stock.");
        payment.MarkRefundPending();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await paymentJobs.EnqueueRefundRequiredHandling(
            payment.Id,
            order.Id,
            cancellationToken);

        logger.LogWarning(
            "Order {OrderId} cancelled due to insufficient stock after successful payment. Payment {PaymentId} marked as refund pending.",
            order.Id,
            payment.Id);

        return Result.Success();
    }

    private async Task PublishOrderPlacedNotificationAsync(
        Order order,
        CancellationToken cancellationToken)
    {
        await publisher.Publish(
            new OrderConfirmedNotification(order.Id, order.CustomerId),
            cancellationToken);
    }
}