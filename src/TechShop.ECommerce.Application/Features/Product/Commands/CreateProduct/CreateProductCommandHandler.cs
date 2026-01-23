namespace TechShop.ECommerce.Application.Features.Product.Commands.CreateProduct;

public class CreateProductCommandHandler(IMapper mapper,
    IProductRepository productRepository) : IRequestHandler<CreateProductCommand, int>
{
    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateProductCommandValidator(productRepository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count != 0)
            throw new BadRequestException("Invalid Product", validationResult);

        var productCreate = mapper.Map<TechShop.ECommerce.Domain.Entities.Product>(request);

        await productRepository.CreateAsync(productCreate);

        return productCreate.Id;
    }
}
