namespace TechShop.ECommerce.Application.Models.Identity;

public class RegistrationRequest
{
    [Required]
    public string FirstName { get; set; } = default!;

    [Required]
    public string LastName { get; set; } = default!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    [MinLength(6)]
    public string UserName { get; set; } = default!;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = default!;
}
