using TechShop.ECommerce.Domain.Entities.Payments;

namespace TechShop.ECommerce.Application.Features.Payments.StripeWebhook;

public sealed class Handler(
    IPaymentRepository paymentRepository,
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork,
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
            return Result.Success();

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

        order.Confirm();

        await unitOfWork.SaveChangesAsync(token);

        logger.LogInformation(
            message: "Payment succeeded for Order {OrderId}",
            args: command.OrderId);

        return Result.Success();
    }
}