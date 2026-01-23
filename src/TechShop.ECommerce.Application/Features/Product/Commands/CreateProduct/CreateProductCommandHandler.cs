namespace TechShop.ECommerce.Application.Features.Product.Commands.CreateProduct;

public class CreateProductCommandHandler(IMapper mapper,
    IProductRepository productRepository) : IRequestHandler<CreateProductCommand, int>
{
    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productCreate = mapper.Map<TechShop.ECommerce.Domain.Entities.Product>(request);

        await productRepository.CreateAsync(productCreate);

        return productCreate.Id;
    }
}
