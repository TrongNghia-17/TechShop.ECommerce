namespace TechShop.ECommerce.Application.Features.Products.UploadProductImage;

public sealed record UploadProductImageCommand(
    Guid ProductId,
    Stream FileStream,
    string FileName,
    string ContentType,
    long FileSize) : IRequest<Result>;
