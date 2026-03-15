using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TechShop.ECommerce.Application.Contracts.Documents;
using TechShop.ECommerce.Application.Features.Orders.Invoices;

namespace TechShop.ECommerce.Infrastructure.Documents;

public sealed class InvoicePdfGenerator : IInvoicePdfGenerator
{
    public byte[] Generate(OrderInvoiceModel model)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header().Column(column =>
                {
                    column.Spacing(4);

                    column.Item().Text("TechShop Invoice")
                        .FontSize(20)
                        .Bold();

                    column.Item().Text($"Invoice Number: {model.InvoiceNumber}");
                    column.Item().Text($"Order Date: {model.OrderDate:yyyy-MM-dd HH:mm}");
                    column.Item().Text($"Customer Email: {model.CustomerEmail}");
                    column.Item().Text($"Shipping Address: {model.ShippingAddress}");

                    if (!string.IsNullOrWhiteSpace(model.Notes))
                    {
                        column.Item().Text($"Notes: {model.Notes}");
                    }
                });

                page.Content().PaddingVertical(20).Column(column =>
                {
                    column.Spacing(12);

                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(4);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(HeaderCellStyle).Text("Product");
                            header.Cell().Element(HeaderCellStyle).AlignRight().Text("Qty");
                            header.Cell().Element(HeaderCellStyle).AlignRight().Text("Unit Price");
                            header.Cell().Element(HeaderCellStyle).AlignRight().Text("Line Total");
                        });

                        foreach (var item in model.Items)
                        {
                            table.Cell().Element(BodyCellStyle).Text(item.ProductName);
                            table.Cell().Element(BodyCellStyle).AlignRight().Text(item.Quantity.ToString());
                            table.Cell().Element(BodyCellStyle).AlignRight().Text($"{item.UnitPrice:N0} VND");
                            table.Cell().Element(BodyCellStyle).AlignRight().Text($"{item.LineTotal:N0} VND");
                        }
                    });

                    column.Item()
                        .AlignRight()
                        .PaddingTop(10)
                        .Text($"Total Amount: {model.TotalAmount:N0} VND")
                        .FontSize(13)
                        .Bold();
                });

                page.Footer()
                    .AlignCenter()
                    .Text("Thank you for shopping with TechShop")
                    .FontSize(9)
                    .FontColor(Colors.Grey.Darken1);
            });
        }).GeneratePdf();
    }

    private static IContainer HeaderCellStyle(IContainer container)
    {
        return container
            .Background(Colors.Grey.Lighten3)
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten1)
            .PaddingVertical(6)
            .PaddingHorizontal(4)
            .DefaultTextStyle(x => x.Bold());
    }

    private static IContainer BodyCellStyle(IContainer container)
    {
        return container
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten2)
            .PaddingVertical(6)
            .PaddingHorizontal(4);
    }
}