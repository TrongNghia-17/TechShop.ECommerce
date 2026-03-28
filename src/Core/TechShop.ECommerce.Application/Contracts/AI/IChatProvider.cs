namespace TechShop.ECommerce.Application.Contracts.AI;

public interface IChatProvider
{
    Task<string> ChatAsync(string prompt, CancellationToken cancellationToken = default);
}
