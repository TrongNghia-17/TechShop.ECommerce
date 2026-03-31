using TechShop.ECommerce.Domain.Entities.Orders;

namespace TechShop.ECommerce.Application.Contracts.Emails;

public interface IOrderConfirmationEmailBuilder
{
    EmailMessage Build(Order order);
}