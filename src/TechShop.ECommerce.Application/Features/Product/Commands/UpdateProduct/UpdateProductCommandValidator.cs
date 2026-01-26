namespace TechShop.ECommerce.Application.Features.Product.Commands.UpdateProduct;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator(IProductRepository productRepository)
    {
        RuleFor(x => x.Id)
            .MustAsync(async (id, _) =>
                await productRepository.ExistsAsync(id))
            .WithMessage("Product does not exist.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MustAsync(async (name, _) =>
                !await productRepository.ExistsByNameAsync(name))
            .WithMessage("Product name already exists.");

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.CategoryId)
            .GreaterThan(0);
    }
}
