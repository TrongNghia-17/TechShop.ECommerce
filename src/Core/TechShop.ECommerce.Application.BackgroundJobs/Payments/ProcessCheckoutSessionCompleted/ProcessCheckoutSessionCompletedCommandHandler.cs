using MediatR;
using Microsoft.Extensions.Logging;
using TechShop.ECommerce.Application.Common;
using TechShop.ECommerce.Application.Contracts.Jobs;
using TechShop.ECommerce.Application.Contracts.Persistence;
using TechShop.ECommerce.Application.Features.Orders.Notifications;
using TechShop.ECommerce.Domain.Entities.Orders;
using TechShop.ECommerce.Domain.Entities.Payments;
using TechShop.ECommerce.Domain.Errors;
using TechShop.ECommerce.Domain.Exceptions;

namespace TechShop.ECommerce.Application.BackgroundJobs.Payments.ProcessCheckoutSessionCompleted;

public sealed class ProcessCheckoutSessionCompletedCommandHandler(
    IPaymentRepository paymentRepository,
    IOrderRepository orderRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    IPaymentJobs paymentJobs,
    IPublisher publisher,
    ILogger<ProcessCheckoutSessionCompletedCommandHandler> logger)
    : IRequestHandler<ProcessCheckoutSessionCompletedCommand, Result>
{
    private enum StockProcessingResult
    {
        ReadyToConfirm = 0,
        CancelledAfterPayment = 1
    }

    public async Task<Result> Handle(
        ProcessCheckoutSessionCompletedCommand command,
        CancellationToken cancellationToken)
    {
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

        await PublishOrderConfirmedNotificationAsync(order, cancellationToken);

        logger.LogInformation(
            "Order {OrderId} confirmed and stock deducted",
            order.Id);

        return Result.Success();
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
                "Duplicate or already processed checkout completion ignored for session {SessionId}",
                sessionId);

            return true;
        }

        return false;
    }

    private async Task<Result<Payment>> GetPaymentAsync(
        string sessionId,
        CancellationToken cancellationToken)
    {
        var payment = await paymentRepository.GetBySessionIdAsync(
            sessionId,
            cancellationToken);

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
        var order = await orderRepository.GetByIdAsync(
            orderId,
            cancellationToken);

        if (order is null)
        {
            logger.LogError(
                "Order {OrderId} not found while processing checkout completion",
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

    private async Task HandleOutOfStockAfterSuccessfulPaymentAsync(
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
    }

    private async Task PublishOrderConfirmedNotificationAsync(
        Order order,
        CancellationToken cancellationToken)
    {
        await publisher.Publish(
            new OrderConfirmedNotification(order.Id, order.CustomerId),
            cancellationToken);
    }
}