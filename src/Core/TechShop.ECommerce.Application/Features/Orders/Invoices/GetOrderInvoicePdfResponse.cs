namespace TechShop.ECommerce.Application.Features.Orders.Invoices;

public sealed record GetOrderInvoicePdfResponse(
    byte[] Content,
    string FileName,
    string ContentType);
