namespace TechShop.ECommerce.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IMediator mediator) : ControllerBase
{
    // GET: api/<ProductsController>
    [HttpGet]
    public async Task<List<ProductDto>> Get()
    {
        var products = await mediator.Send(new GetProductsQuery());
        return products;
    }

    // GET api/<ProductsController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDetailsDto>> Get(int id)
    {
        var product = await mediator.Send(new GetProductDetailsQuery(id));
        return Ok(product);
    }

    // POST api/<ProductsController>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Post(CreateProductCommand command)
    {
        var response = await mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = response });
    }

    // PUT api/<ProductsController>/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(400)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Put(UpdateProductCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    // DELETE api/<ProductsController>/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteProductCommand { Id = id };
        await mediator.Send(command);
        return NoContent();
    }
}
