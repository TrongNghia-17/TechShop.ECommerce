namespace TechShop.ECommerce.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }
    public DateTimeOffset DateCreated { get; protected set; }
    public string? CreatedBy { get; protected set; }

    public DateTimeOffset? DateModified { get; protected set; }
    public string? ModifiedBy { get; protected set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    internal void MarkAsCreated(string? userId)
    {
        DateCreated = DateTimeOffset.UtcNow;
        CreatedBy = userId;
    }

    internal void MarkAsUpdated(string? userId)
    {
        DateModified = DateTimeOffset.UtcNow;
        ModifiedBy = userId;
    }
}