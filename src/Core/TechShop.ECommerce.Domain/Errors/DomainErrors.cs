namespace TechShop.ECommerce.Domain.Errors;

public record DomainErrors(string Code, string Description)
{
    public static readonly DomainErrors None = new(string.Empty, string.Empty);

    public static DomainErrors NotFound(string code, string description) =>
        new NotFoundError(code, description);

    public static DomainErrors Validation(string code, string description) =>
        new ValidationError(code, description);

    public static DomainErrors Conflict(string code, string description) =>
        new ConflictError(code, description);

    public static DomainErrors Unauthorized(string code, string description) =>
        new UnauthorizedError(code, description);

    public static DomainErrors Failure(string code, string description) =>
        new(code, description);
}

public sealed record NotFoundError(string Code, string Description)
    : DomainErrors(Code, Description);

public sealed record ValidationError(string Code, string Description)
    : DomainErrors(Code, Description);

public sealed record ConflictError(string Code, string Description)
    : DomainErrors(Code, Description);

public sealed record UnauthorizedError(string Code, string Description)
    : DomainErrors(Code, Description);
