using TechShop.ECommerce.Application.Features.Orders.Invoices;

namespace TechShop.ECommerce.Application.Contracts.Documents;

public interface IInvoicePdfGenerator
{
    byte[] Generate(OrderInvoiceModel model);
}
