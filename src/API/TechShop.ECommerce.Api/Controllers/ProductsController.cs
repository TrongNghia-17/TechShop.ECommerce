namespace TechShop.ECommerce.Api.Controllers;

[Route("api/[controller]")]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductDetailsDto>> GetById(Guid id, CancellationToken ct)
    {
        var dto = await mediator.Send(new GetProductDetailsQuery(id), ct);

        return Ok(dto);
    }
}
