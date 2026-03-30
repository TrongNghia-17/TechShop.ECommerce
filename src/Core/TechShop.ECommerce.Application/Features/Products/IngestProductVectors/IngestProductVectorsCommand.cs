using TechShop.ECommerce.Application.Common.Results;

namespace TechShop.ECommerce.Application.Features.Products.IngestProductVectors;

public sealed record IngestProductVectorsCommand : IRequest<Result<int>>;
