using MediatR;
using Microsoft.Extensions.Logging;
using TechShop.ECommerce.Application.Contracts.Persistence;

namespace TechShop.ECommerce.Application.BackgroundJobs.Orders.ExpirePendingOrders;

public sealed class ExpirePendingOrdersCommandHandler(
    IOrderRepository orderRepository,
    IPaymentRepository paymentRepository,
    IUnitOfWork unitOfWork,
    ILogger<ExpirePendingOrdersCommandHandler> logger)
    : IRequestHandler<ExpirePendingOrdersCommand, int>
{
    private const int ExpireAfterMinutes = 30;

    public async Task<int> Handle(
        ExpirePendingOrdersCommand command,
        CancellationToken cancellationToken)
    {
        var cutoffUtc = DateTimeOffset.UtcNow.AddMinutes(-ExpireAfterMinutes);

        var orders = await orderRepository.GetPendingOrdersCreatedBeforeAsync(
            cutoffUtc,
            cancellationToken);

        if (orders.Count == 0)
        {
            logger.LogInformation(
                "No pending orders older than {ExpireAfterMinutes} minutes were found",
                ExpireAfterMinutes);

            return 0;
        }

        foreach (var order in orders)
        {
            order.Expire();
        }

        var orderIds = orders
            .Select(order => order.Id)
            .ToList();

        var payments = await paymentRepository.GetPendingByOrderIdsAsync(
            orderIds,
            cancellationToken);

        foreach (var payment in payments)
        {
            payment.Expire();
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Expired {OrderCount} pending orders and {PaymentCount} pending payments older than {ExpireAfterMinutes} minutes",
            orders.Count,
            payments.Count,
            ExpireAfterMinutes);

        return orders.Count;
    }
}
