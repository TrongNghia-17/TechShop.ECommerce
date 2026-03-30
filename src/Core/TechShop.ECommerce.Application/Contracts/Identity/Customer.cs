namespace TechShop.ECommerce.Application.Contracts.Identity;

public sealed record Customer(
    Guid Id,
    string Email
);
