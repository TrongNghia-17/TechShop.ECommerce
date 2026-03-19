using TechShop.ECommerce.Application.Exceptions;

namespace TechShop.ECommerce.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .GroupBy(
                    x => x.PropertyName,
                    x => x.ErrorMessage)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToArray());

            if (failures.Any())
                throw new BadRequestException("Validation failed", failures);
        }

        return await next();
    }
}
