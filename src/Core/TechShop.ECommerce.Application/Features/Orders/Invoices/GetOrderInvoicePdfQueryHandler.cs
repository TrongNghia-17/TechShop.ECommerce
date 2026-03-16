using TechShop.ECommerce.Application.Contracts.Documents;

namespace TechShop.ECommerce.Application.Features.Orders.Invoices;

public sealed class GetOrderInvoicePdfQueryHandler(
    IOrderRepository orderRepository,
    IInvoicePdfGenerator invoicePdfGenerator,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetOrderInvoicePdfQuery, GetOrderInvoicePdfResponse>
{
    public async Task<GetOrderInvoicePdfResponse> Handle(
        GetOrderInvoicePdfQuery request,
        CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId;

        var order = await orderRepository.GetByIdWithItemsAsync(
            request.OrderId,
            cancellationToken)
            ?? throw new NotFoundException(nameof(Order), request.OrderId);

        if (order.CustomerId != currentUserId)
            throw new NotFoundException(nameof(Order), request.OrderId);

        if (order.Status != OrderStatus.Confirmed)
            throw new BadRequestException("Invoice PDF is only available for confirmed orders.");

        var model = new OrderInvoiceModel(
            InvoiceNumber: order.Id.ToString("N").ToUpperInvariant(),
            OrderDate: order.OrderDate,
            CustomerEmail: order.CustomerEmail,
            ShippingAddress: FormatAddress(order.ShippingAddress),
            TotalAmount: order.TotalAmount,
            Items: order.OrderItems
                .Select(item => new OrderInvoiceItemModel(
                    item.ProductName,
                    item.Quantity,
                    item.UnitPrice,
                    item.UnitPrice * item.Quantity))
                .ToList(),
            Notes: order.Notes);

        var pdfBytes = invoicePdfGenerator.Generate(model);

        return new GetOrderInvoicePdfResponse(
            Content: pdfBytes,
            FileName: $"invoice-{order.Id:N}.pdf",
            ContentType: "application/pdf");
    }

    private static string FormatAddress(Address address)
    {
        var parts = new[]
        {
            (string?)address.Street,
            (string?)address.City,
            (string?)address.PostalCode,
            (string?)address.Country
        }
        .Where(static value => !string.IsNullOrWhiteSpace(value));

        return string.Join(", ", parts);
    }
}