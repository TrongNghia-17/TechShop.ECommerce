namespace TechShop.ECommerce.Api.Controllers.v2;

[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
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
        [FromBody] AddToCartRequest request,
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
        [FromBody] RemoveFromCartRequest request,
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

}