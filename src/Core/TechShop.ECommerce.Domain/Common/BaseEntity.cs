namespace TechShop.ECommerce.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; protected set; }
    public DateTime CreatedDate { get; protected set; }
    public DateTime? UpdatedDate { get; protected set; }
    public bool IsDeleted { get; protected set; }

    internal void SetId(int id) => Id = id;

    internal void MarkAsCreated()
    {
        CreatedDate = DateTime.UtcNow;
        IsDeleted = false;
    }

    internal void MarkAsUpdated()
    {
        UpdatedDate = DateTime.UtcNow;
    }

    internal void MarkAsDeleted()
    {
        IsDeleted = true;
        UpdatedDate = DateTime.UtcNow;
    }
}