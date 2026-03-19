using TechShop.ECommerce.Domain.Entities.Orders;

namespace TechShop.ECommerce.Application.Common.Emails;

public interface IOrderConfirmationEmailBuilder
{
    EmailMessage Build(Order order);
}