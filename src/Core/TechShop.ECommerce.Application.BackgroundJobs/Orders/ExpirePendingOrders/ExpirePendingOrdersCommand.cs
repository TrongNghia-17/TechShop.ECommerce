using MediatR;

namespace TechShop.ECommerce.Application.BackgroundJobs.Orders.ExpirePendingOrders;

public sealed record ExpirePendingOrdersCommand : IRequest<int>;