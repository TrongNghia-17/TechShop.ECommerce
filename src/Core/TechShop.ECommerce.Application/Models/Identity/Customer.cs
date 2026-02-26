namespace TechShop.ECommerce.Application.Models.Identity;

public sealed record Customer(
    Guid Id,
    string Email,
    string Firstname,
    string Lastname
);
