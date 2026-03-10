namespace TechShop.ECommerce.Infrastructure.BackgroundJobs.Orders;

public interface IHangfireOrderMaintenanceJobExecutor
{
    Task ExpirePendingOrders();
}
