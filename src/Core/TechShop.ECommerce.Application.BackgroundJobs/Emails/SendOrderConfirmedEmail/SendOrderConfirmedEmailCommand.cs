using MediatR;

namespace TechShop.ECommerce.Application.BackgroundJobs.Emails.SendOrderConfirmedEmail;

public sealed record SendOrderConfirmedEmailCommand(Guid OrderId) : IRequest;
