namespace TechShop.ECommerce.Application.Features.Products.Commands.BulkPurge;

public sealed class BulkPurgeProductsCommandHandler(
    IProductRepository productRepository,
    IAppLogger<BulkPurgeProductsCommandHandler> logger)
    : IRequestHandler<BulkPurgeProductsCommand, int>
{
    public async Task<int> Handle(BulkPurgeProductsCommand request, CancellationToken cancellationToken)
    {
        var thresholdDate = DateTimeOffset.UtcNow.AddDays(-request.DaysOld);

        logger.LogInformation(
            "Starting bulk purge for products deleted before {ThresholdDate} (older than {Days} days)",
            thresholdDate,
            request.DaysOld);

        var deletedCount = await productRepository.DeleteSoftDeletedProductsAsync(thresholdDate, cancellationToken);

        logger.LogWarning(
            "COMPLETED: Permanently deleted {Count} products from the database.",
            deletedCount);

        return deletedCount;
    }
}
