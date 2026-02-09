namespace TechShop.ECommerce.Domain.Common;

public interface ISoftDelete
{
    bool IsDeleted { get; }
    DateTimeOffset? DateDeleted { get; }
    string? DeletedBy { get; }

    void MarkAsDeleted(string? userId);
    void Restore(string? userId);
}
