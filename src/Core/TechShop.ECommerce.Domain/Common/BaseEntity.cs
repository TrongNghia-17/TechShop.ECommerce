namespace TechShop.ECommerce.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; protected set; }
    public DateTime DateCreated { get; protected set; }
    public string? CreatedBy { get; protected set; }
    public DateTime? DateModified { get; protected set; }
    public string? ModifiedBy { get; protected set; }
    public bool IsDeleted { get; protected set; }

    internal void SetId(int id) => Id = id;

    internal void MarkAsCreated(string? userId)
    {
        DateCreated = DateTime.UtcNow;
        CreatedBy = userId;
        IsDeleted = false;
    }

    internal void MarkAsUpdated(string? userId)
    {
        DateModified = DateTime.UtcNow;
        ModifiedBy = userId;
    }

    internal void MarkAsDeleted()
    {
        IsDeleted = true;
        DateModified = DateTime.UtcNow;
    }
}