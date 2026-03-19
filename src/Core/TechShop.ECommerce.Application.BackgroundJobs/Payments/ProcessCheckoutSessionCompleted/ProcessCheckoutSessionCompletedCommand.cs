using MediatR;
using TechShop.ECommerce.Application.Common.Results;

namespace TechShop.ECommerce.Application.BackgroundJobs.Payments.ProcessCheckoutSessionCompleted;

public sealed record ProcessCheckoutSessionCompletedCommand(
    string SessionId,
    Guid OrderId) : IRequest<Result>;
