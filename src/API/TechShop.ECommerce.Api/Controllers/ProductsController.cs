namespace TechShop.ECommerce.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductDto>>> Get()
    {
        var products = await mediator.Send(new GetProductsQuery());
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDetailsDto>> Get(int id)
    {
        var product = await mediator.Send(new GetProductDetailsQuery(id));
        return Ok(product);
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Post(CreateProductCommand command)
    {
        var id = await mediator.Send(command);
        return CreatedAtAction(
            nameof(Get),
            new { id },
            new { id }
        );

    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(400)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Put(int id, UpdateProductCommand command)
    {
        if (command.Id != 0 && command.Id != id)
        {
            return BadRequest("Route id and body id do not match");
        }

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
