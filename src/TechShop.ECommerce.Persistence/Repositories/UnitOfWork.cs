namespace TechShop.ECommerce.Persistence.Repositories;

public class UnitOfWork(TechShopDatabaseContext context) : IUnitOfWork
{
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}

