namespace TechShop.ECommerce.Domain.Abstractions;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }
    public DateTimeOffset DateCreated { get; protected set; }
    public Guid? CreatedBy { get; protected set; }

    public DateTimeOffset? DateModified { get; protected set; }
    public Guid? ModifiedBy { get; protected set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    internal void MarkAsCreated(Guid? userId)
    {
        DateCreated = DateTimeOffset.UtcNow;
        CreatedBy = userId;
    }

    internal void MarkAsUpdated(Guid? userId)
    {
        DateModified = DateTimeOffset.UtcNow;
        ModifiedBy = userId;
    }
}