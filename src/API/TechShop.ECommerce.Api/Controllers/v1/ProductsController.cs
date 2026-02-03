namespace TechShop.ECommerce.Api.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> Get()
        => Ok(await mediator.Send(new GetProductsQuery()));

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDetailsDto>> Get(int id)
        => Ok(await mediator.Send(new GetProductDetailsQuery(id)));

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Post(CreateProductCommand command)
    {
        var id = await mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id, version = "1" }, new { id });
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(400)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Put(int id, UpdateProductCommand command)
    {
        var updateCommand = command with { Id = id };
        await mediator.Send(updateCommand);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Delete(int id)
    {
        await mediator.Send(new DeleteProductCommand(id));
        return NoContent();
    }
}
