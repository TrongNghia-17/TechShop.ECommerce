using TechShop.ECommerce.Application.Contracts.Storage;

namespace TechShop.ECommerce.Application.Features.Products.UploadProductImage;

public sealed class UploadProductImageCommandHandler(
    IProductRepository productRepository,
    IFileStorage fileStorage,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UploadProductImageCommand, Result>
{
    public async Task<Result> Handle(
        UploadProductImageCommand command,
        CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(
            command.ProductId,
            cancellationToken);

        if (product is null)
            return ProductErrors.NotFound(command.ProductId);

        var fileExtension = Path.GetExtension(command.FileName);

        if (!string.IsNullOrWhiteSpace(product.MainImageBlobName))
        {
            await fileStorage.DeleteAsync(
                product.MainImageBlobName,
                cancellationToken);
        }

        var uploadedBlobName = await fileStorage.UploadProductImageAsync(
            command.ProductId,
            command.FileStream,
            command.ContentType,
            fileExtension,
            cancellationToken);

        product.UpdateMainImage(uploadedBlobName);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
