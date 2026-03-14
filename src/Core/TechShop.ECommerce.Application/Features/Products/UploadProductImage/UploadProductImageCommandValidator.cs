namespace TechShop.ECommerce.Application.Features.Products.UploadProductImage;

public sealed class UploadProductImageCommandValidator
    : AbstractValidator<UploadProductImageCommand>
{
    private const long MaxFileSizeInBytes = 5 * 1024 * 1024;

    private static readonly HashSet<string> AllowedExtensions =
    [
        ".jpg", ".jpeg", ".png", ".webp"
    ];

    private static readonly HashSet<string> AllowedContentTypes =
    [
        "image/jpeg", "image/png", "image/webp"
    ];

    public UploadProductImageCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty();

        RuleFor(x => x.FileStream)
            .NotNull();

        RuleFor(x => x.FileName)
            .NotEmpty()
            .Must(fileName =>
            {
                var extension = Path.GetExtension(fileName).ToLowerInvariant();
                return AllowedExtensions.Contains(extension);
            })
            .WithMessage("Only .jpg, .jpeg, .png and .webp files are allowed.");

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .Must(contentType => AllowedContentTypes.Contains(contentType))
            .WithMessage("Invalid image content type.");

        RuleFor(x => x.FileSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(MaxFileSizeInBytes)
            .WithMessage("File size must not exceed 5 MB.");
    }
}
