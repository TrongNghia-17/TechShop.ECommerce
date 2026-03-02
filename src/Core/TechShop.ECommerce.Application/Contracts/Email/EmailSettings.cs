namespace TechShop.ECommerce.Application.Contracts.Email;

public class EmailSettings
{
    [Required]
    public string ApiKey { get; set; } = default!;

    [Required]
    [EmailAddress]
    public string FromAddress { get; set; } = default!;

    [Required]
    public string FromName { get; set; } = default!;
}
