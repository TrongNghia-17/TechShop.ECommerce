namespace TechShop.ECommerce.Api.Models;

public class CustomProblemDetails : ProblemDetails
{
    public string? ErrorCode { get; set; }
    public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
}
