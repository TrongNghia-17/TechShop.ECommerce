namespace TechShop.ECommerce.Domain.Abstractions;

public interface ISoftDelete
{
    bool IsDeleted { get; }
    DateTimeOffset? DateDeleted { get; }
    Guid? DeletedBy { get; }

    void MarkAsDeleted(Guid? userId);
    void Restore(Guid? userId);
}
