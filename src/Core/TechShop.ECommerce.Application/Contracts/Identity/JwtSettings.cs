namespace TechShop.ECommerce.Application.Contracts.Identity;

public class JwtSettings
{
    [Required]
    public string Key { get; set; } = default!;

    [Required]
    public string Issuer { get; set; } = default!;

    [Required]
    public string Audience { get; set; } = default!;

    [Range(1, 1440)]
    public int DurationInMinutes { get; set; }
}
