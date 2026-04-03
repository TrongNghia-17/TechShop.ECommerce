using TechShop.ECommerce.Application.BackgroundJobs.Orders.ExpirePendingOrders;

namespace TechShop.ECommerce.Infrastructure.BackgroundJobs.Orders;

public sealed class HangfireOrderMaintenanceJobExecutor(
    ISender sender,
    ILogger<HangfireOrderMaintenanceJobExecutor> logger)
    : IHangfireOrderMaintenanceJobExecutor
{
    public async Task ExpirePendingOrders()
    {
        logger.LogInformation("Executing recurring job: ExpirePendingOrders");

        var expiredCount = await sender.Send(new ExpirePendingOrdersCommand());

        logger.LogInformation(
            "Recurring job ExpirePendingOrders completed. Expired {Count} orders",
            expiredCount);
    }
}