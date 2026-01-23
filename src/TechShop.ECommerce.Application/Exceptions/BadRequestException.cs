namespace TechShop.ECommerce.Application.Exceptions;

public class BadRequestException : Exception
{
    public IReadOnlyList<string> ValidationErrors { get; }
    public BadRequestException(string message) : base(message)
    {
        ValidationErrors = [];
    }

    public BadRequestException(string message, ValidationResult validationResult) : base(message)
    {
        ValidationErrors = validationResult.Errors
            .Select(e => e.ErrorMessage)
            .ToList()
            .AsReadOnly();
    }
}
