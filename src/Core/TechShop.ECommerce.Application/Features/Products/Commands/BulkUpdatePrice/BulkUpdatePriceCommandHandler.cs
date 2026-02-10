namespace TechShop.ECommerce.Application.Features.Products.Commands.BulkUpdatePrice;

public sealed class BulkUpdatePriceCommandHandler(
    IProductRepository productRepository,
    IUserService userService,
    IAppLogger<BulkUpdatePriceCommandHandler> logger)
    : IRequestHandler<BulkUpdatePriceCommand, Unit>
{
    public async Task<Unit> Handle(BulkUpdatePriceCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting bulk update price for Category {CategoryId}", request.CategoryId);

        var multiplier = 1 + (request.PercentageChange / 100m);

        var currentUserId = userService.UserId ?? "System_BulkUpdate";

        await productRepository.UpdatePriceByCategoryAsync(
            request.CategoryId,
            multiplier,
            currentUserId,
            cancellationToken);

        logger.LogInformation("Completed bulk update.");
        return Unit.Value;
    }
}
