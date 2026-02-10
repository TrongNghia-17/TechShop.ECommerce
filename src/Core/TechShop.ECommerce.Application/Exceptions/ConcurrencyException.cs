namespace TechShop.ECommerce.Application.Exceptions;

public sealed class ConcurrencyException : Exception
{
    public ConcurrencyException(string message)
        : base(message) { }

    public ConcurrencyException(string message, Exception inner)
        : base(message, inner) { }
}
