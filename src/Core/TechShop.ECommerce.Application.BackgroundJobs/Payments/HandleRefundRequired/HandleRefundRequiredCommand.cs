using MediatR;

namespace TechShop.ECommerce.Application.BackgroundJobs.Payments.HandleRefundRequired;

public sealed record HandleRefundRequiredCommand(
    Guid PaymentId,
    Guid OrderId) : IRequest;
