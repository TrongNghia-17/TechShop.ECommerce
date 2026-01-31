namespace TechShop.ECommerce.Application.Exceptions;

public class BadRequestException : Exception
{
    public IDictionary<string, string[]> ValidationErrors { get; set; }
        = new Dictionary<string, string[]>();

    public BadRequestException(string message)
       : base(message)
    {
    }

    public BadRequestException(
        string message,
        IDictionary<string, string[]> validationErrors)
        : base(message)
    {
        ValidationErrors = validationErrors;
    }


    public BadRequestException(
    string message,
    FluentValidation.Results.ValidationResult validationResult)
    : base(message)
    {
        ValidationErrors = validationResult.ToDictionary();
    }
}
