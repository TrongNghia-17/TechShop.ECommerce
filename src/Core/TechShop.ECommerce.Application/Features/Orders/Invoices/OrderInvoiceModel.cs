namespace TechShop.ECommerce.Application.Features.Orders.Invoices;

public sealed record OrderInvoiceModel(
    string InvoiceNumber,
    DateTimeOffset OrderDate,
    string CustomerEmail,
    string ShippingAddress,
    decimal TotalAmount,
    IReadOnlyCollection<OrderInvoiceItemModel> Items,
    string? Notes);

public sealed record OrderInvoiceItemModel(
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal LineTotal);
