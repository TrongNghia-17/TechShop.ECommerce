namespace TechShop.ECommerce.Application.Features.Products.Commands.Create;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    private readonly IProductRepository _productRepository;
    public CreateProductCommandValidator(IProductRepository productRepository)
    {
        _productRepository = productRepository;

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .NotNull()
            .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters")
            .MinimumLength(3).WithMessage("{PropertyName} must be at least 3 characters");

        RuleFor(p => p.Summary)
            .MaximumLength(500).WithMessage("{PropertyName} must not exceed 500 characters")
            .When(p => !string.IsNullOrEmpty(p.Summary));

        RuleFor(p => p.Description)
            .MaximumLength(4000).WithMessage("{PropertyName} must not exceed 4000 characters")
            .When(p => !string.IsNullOrEmpty(p.Description));

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0")
            .LessThanOrEqualTo(1000000).WithMessage("{PropertyName} cannot exceed 1,000,000")
            .PrecisionScale(18, 2, false).WithMessage("{PropertyName} must have maximum 18 digits and 2 decimal places");

        RuleFor(p => p)
            .MustAsync(ProductNameUnique)
            .WithMessage("Product already exists");
    }

    private async Task<bool> ProductNameUnique(
         CreateProductCommand command,
         CancellationToken token)
    {
        return !await _productRepository.ExistsByNameAsync(command.Name);
    }
}
