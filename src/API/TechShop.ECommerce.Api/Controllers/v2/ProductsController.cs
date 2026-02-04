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

    [HttpGet("{id}")]
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
    public async Task<ActionResult> Post(
        CreateProductCommand command,
        [FromServices] IOutputCacheStore cache,
        CancellationToken token)
    {
        var id = await mediator.Send(command, token);
        await cache.EvictByTagAsync("products", token);
        return CreatedAtAction(nameof(Get), new { id, version = "2" }, new { id });
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(400)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    [EnableRateLimiting("ProductsWrite")]
    public async Task<ActionResult> Put(
        Guid id,
        UpdateProductCommand command,
        [FromServices] IOutputCacheStore cache,
        CancellationToken token)
    {
        await mediator.Send(command with { Id = id }, token);
        await cache.EvictByTagAsync("products", token);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    [EnableRateLimiting("ProductsWrite")]
    public async Task<ActionResult> Delete(
        Guid id,
        [FromServices] IOutputCacheStore cache,
        CancellationToken token)
    {
        await mediator.Send(new DeleteProductCommand(id), token);
        await cache.EvictByTagAsync("products", token);
        return NoContent();
    }
}
