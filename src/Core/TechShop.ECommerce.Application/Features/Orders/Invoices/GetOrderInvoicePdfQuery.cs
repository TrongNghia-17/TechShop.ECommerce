namespace TechShop.ECommerce.Application.Features.Orders.Invoices;

public sealed record GetOrderInvoicePdfQuery(Guid OrderId) : IRequest<GetOrderInvoicePdfResponse>;