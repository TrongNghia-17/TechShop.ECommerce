namespace TechShop.ECommerce.Domain.Errors;

public record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static Error NotFound(string code, string description) =>
        new NotFoundError(code, description);

    public static Error Validation(string code, string description) =>
        new ValidationError(code, description);

    public static Error Conflict(string code, string description) =>
        new ConflictError(code, description);

    public static Error Unauthorized(string code, string description) =>
        new UnauthorizedError(code, description);

    public static Error Failure(string code, string description) =>
        new(code, description);
}

public sealed record NotFoundError(string Code, string Description)
    : Error(Code, Description);

public sealed record ValidationError(string Code, string Description)
    : Error(Code, Description);

public sealed record ConflictError(string Code, string Description)
    : Error(Code, Description);

public sealed record UnauthorizedError(string Code, string Description)
    : Error(Code, Description);
