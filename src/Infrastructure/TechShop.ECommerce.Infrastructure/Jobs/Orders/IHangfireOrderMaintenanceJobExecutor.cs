namespace TechShop.ECommerce.Infrastructure.Jobs.Orders;

public interface IHangfireOrderMaintenanceJobExecutor
{
    Task ExpirePendingOrders();
}
