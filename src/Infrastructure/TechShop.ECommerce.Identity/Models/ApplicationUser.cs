namespace TechShop.ECommerce.Identity.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;

    public ICollection<RefreshToken> RefreshTokens { get; private set; } = [];

    public void UpdateName(string firstName, string lastName)
    {
        FirstName = NormalizeRequired(firstName, nameof(firstName));
        LastName = NormalizeRequired(lastName, nameof(lastName));
    }

    private static string NormalizeRequired(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{paramName} is required.", paramName);
        }

        return value.Trim();
    }
}