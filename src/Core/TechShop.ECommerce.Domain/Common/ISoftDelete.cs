namespace TechShop.ECommerce.Domain.Common;

public interface ISoftDelete
{
    bool IsDeleted { get; }

    void MarkAsDeleted(string? userId);
    void Restore(string? userId);
}
