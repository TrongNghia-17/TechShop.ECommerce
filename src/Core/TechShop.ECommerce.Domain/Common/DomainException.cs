namespace TechShop.ECommerce.Domain.Common;

public class DomainException(string message) : Exception(message)
{
}