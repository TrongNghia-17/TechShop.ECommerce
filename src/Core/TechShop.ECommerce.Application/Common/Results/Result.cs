using TechShop.ECommerce.Domain.Errors;

namespace TechShop.ECommerce.Application.Common.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public DomainErrors Error { get; }

    protected Result(bool isSuccess, DomainErrors error)
    {
        if (isSuccess && error != DomainErrors.None)
        {
            throw new ArgumentException("A successful result cannot contain an error.", nameof(error));
        }

        if (!isSuccess && error == DomainErrors.None)
        {
            throw new ArgumentException("A failed result must contain an error.", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, DomainErrors.None);
    public static Result Failure(DomainErrors error) => new(false, error);

    public static implicit operator Result(DomainErrors error) => Failure(error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failed result cannot be accessed.");

    private Result(TValue value) : base(true, DomainErrors.None)
    {
        _value = value;
    }

    private Result(DomainErrors error) : base(false, error)
    {
        _value = default;
    }

    public static Result<TValue> Success(TValue value) => new(value);
    public new static Result<TValue> Failure(DomainErrors error) => new(error);

    public static implicit operator Result<TValue>(TValue value) => Success(value);
    public static implicit operator Result<TValue>(DomainErrors error) => Failure(error);
}

