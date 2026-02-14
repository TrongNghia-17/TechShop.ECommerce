namespace TechShop.ECommerce.Api.Controllers.v2;

[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [OutputCache(PolicyName = "ProductsList")]
    [EnableRateLimiting("ProductsSliding")]
    public async Task<ActionResult<PagedResult<ProductDto>>> Get(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] Guid? categoryId = null,
        [FromQuery] string? sort = "price")
    {
        return Ok(await mediator.Send(new GetProductsPagedQuery(pageNumber, pageSize, categoryId, sort)));
    }

    [HttpGet("cursor")]
    [EnableRateLimiting("ProductsSliding")]
    public async Task<ActionResult<CursorPagedResult<ProductDto>>> GetCursor(
        [FromQuery] string? search = null,
        [FromQuery] string? after = null,
        [FromQuery] int limit = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(
            new GetProductsCursorQuery(search, after, limit),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status304NotModified)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDetailsDto>> GetById(Guid id, CancellationToken ct)
    {
        var dto = await mediator.Send(new GetProductDetailsQuery(id), ct);

        var lastModifiedUtc = (dto.DateModified ?? dto.DateCreated).ToUniversalTime();
        var etag = HttpCachingExtensions.BuildWeakEtag(dto.Id, lastModifiedUtc);

        if (Request.IsNotModified(etag))
            return StatusCode(StatusCodes.Status304NotModified);

        Response.ApplyCacheHeaders(etag, maxAgeSeconds: 60);
        return Ok(dto);
    }


    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [EnableRateLimiting("ProductsWrite")]
    public async Task<ActionResult> BulkUpdatePrice(
        [FromBody] BulkUpdatePriceCommand command,
        [FromServices] IOutputCacheStore cache,
        CancellationToken token)
    {
        await mediator.Send(command, token);

        await cache.EvictByTagAsync("products", token);

        return NoContent();
    }

    /// <summary>
    /// Permanently deletes soft-deleted products older than the specified number of days.
    /// </summary>
    /// <param name="daysOld">Minimum age of deleted status (e.g., 30 days).</param>
    /// <returns>Number of records deleted.</returns>
    [HttpDelete("bulk/purge")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [EnableRateLimiting("ProductsWrite")]
    // [Authorize(Roles = "Admin")] 
    public async Task<ActionResult<int>> BulkPurgeDeleted(
        [FromQuery] int daysOld,
        [FromServices] IOutputCacheStore cache,
        CancellationToken token)
    {
        var command = new BulkPurgeProductsCommand(daysOld);

        var deletedCount = await mediator.Send(command, token);

        await cache.EvictByTagAsync("products", token);

        return Ok(new { Count = deletedCount });
    }
}
