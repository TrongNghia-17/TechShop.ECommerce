using TechShop.ECommerce.Application.Features.Orders.Commands.PlaceOrder;

namespace TechShop.ECommerce.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController(
    IMediator mediator,
    IUserService userService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {

        var command = new PlaceOrderCommand(
            CustomerId: userService.UserId,
            ShippingAddress: request.ShippingAddress,
            Notes: request.Notes
        );

        var orderId = await mediator.Send(command);

        return CreatedAtAction(nameof(GetOrderById), new { id = orderId }, new { id = orderId });
    }

    [HttpGet("{id}")]
    public Task<IActionResult> GetOrderById(Guid id) => throw new NotImplementedException();
}
