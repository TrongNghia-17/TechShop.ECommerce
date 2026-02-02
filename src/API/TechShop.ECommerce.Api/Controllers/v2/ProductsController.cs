namespace TechShop.ECommerce.Api.Controllers.v2;

[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<ProductDto>>> Get(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? categoryId = null,
        [FromQuery] string? sort = "price")
    {
        return Ok(await mediator.Send(new GetProductsPagedQuery(pageNumber, pageSize, categoryId, sort)));
    }
}
