namespace TechShop.ECommerce.Application.Exceptions;

public class NotFoundException(string name, object key)
    : Exception($"{name} with identifier ({key}) was not found.")
{
}
