namespace TechShop.ECommerce.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public sealed class CartsController(
    IMediator mediator,
    IUserService userService) : ControllerBase
{
    [HttpPost("items")]
    [ProducesResponseType(typeof(AddToCartResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AddToCartResult>> AddItem(
        [FromBody] CartItemRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddToCartCommand(
            CustomerId: userService.UserId,
            ProductId: request.ProductId,
            Quantity: request.Quantity
        );

        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("items")]
    [ProducesResponseType(typeof(AddToCartResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AddToCartResult>> RemoveItem(
        [FromBody] CartItemRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RemoveFromCartCommand(
            CustomerId: userService.UserId,
            ProductId: request.ProductId,
            Quantity: request.Quantity
        );

        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetCartResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<GetCartResult>> GetCart(CancellationToken cancellationToken)
    {
        var query = new GetCartQuery(userService.UserId);
        var result = await mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}