using MediatR;
using Microsoft.Extensions.Logging;
using TechShop.ECommerce.Application.Contracts.Persistence;

namespace TechShop.ECommerce.Application.BackgroundJobs.Orders.ExpirePendingOrders;

public sealed class ExpirePendingOrdersCommandHandler(
    IOrderRepository orderRepository,
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

        foreach (var order in orders)
        {
            order.Expire();
        }

        if (orders.Count > 0)
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        logger.LogInformation(
            "Expired {Count} pending orders older than {ExpireAfterMinutes} minutes",
            orders.Count,
            ExpireAfterMinutes);

        return orders.Count;
    }
}
