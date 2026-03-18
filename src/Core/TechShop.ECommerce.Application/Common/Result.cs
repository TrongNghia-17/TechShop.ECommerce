namespace TechShop.ECommerce.Application.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Domain.Errors.DomainErrors Error { get; }

    protected Result(bool isSuccess, Domain.Errors.DomainErrors error)
    {
        if (isSuccess && error != Domain.Errors.DomainErrors.None)
            throw new ArgumentException("Success result cannot have an error", nameof(error));
        if (!isSuccess && error == Domain.Errors.DomainErrors.None)
            throw new ArgumentException("Failure result must have an error", nameof(error));

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Domain.Errors.DomainErrors.None);
    public static Result Failure(Domain.Errors.DomainErrors error) => new(false, error);

    // Implicit conversion from Error to Result
    public static implicit operator Result(Domain.Errors.DomainErrors error) => Failure(error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access value of a failed result");

    private Result(TValue value) : base(true, Domain.Errors.DomainErrors.None)
    {
        _value = value;
    }

    private Result(Domain.Errors.DomainErrors error) : base(false, error)
    {
        _value = default;
    }

    public static Result<TValue> Success(TValue value) => new(value);
    public new static Result<TValue> Failure(Domain.Errors.DomainErrors error) => new(error);

    // Implicit conversions for cleaner syntax
    public static implicit operator Result<TValue>(TValue value) => Success(value);
    public static implicit operator Result<TValue>(Domain.Errors.DomainErrors error) => Failure(error);
}

