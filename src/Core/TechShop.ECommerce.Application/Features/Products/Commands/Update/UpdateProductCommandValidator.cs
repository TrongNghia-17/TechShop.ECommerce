namespace TechShop.ECommerce.Application.Features.Products.Commands.Update;

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
            .MustAsync(async (command, name, _) =>
                !await productRepository.ExistsByNameAsync(name, command.Id))
            .WithMessage("Product name already exists.");

        RuleFor(x => x.Price)
            .GreaterThan(0);
    }
}
