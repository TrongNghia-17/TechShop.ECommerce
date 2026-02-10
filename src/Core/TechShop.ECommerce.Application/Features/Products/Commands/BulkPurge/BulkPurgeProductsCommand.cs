namespace TechShop.ECommerce.Application.Features.Products.Commands.BulkPurge;

public sealed record BulkPurgeProductsCommand(int DaysOld) : IRequest<int>;