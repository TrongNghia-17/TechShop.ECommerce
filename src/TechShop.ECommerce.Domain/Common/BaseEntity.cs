namespace TechShop.ECommerce.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; protected set; }
    public DateTime CreatedDate { get; protected set; }
    public DateTime? UpdatedDate { get; protected set; }
    public bool IsDeleted { get; protected set; }
}