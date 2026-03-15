using TechShop.ECommerce.Application.Contracts.Documents;
using TechShop.ECommerce.Infrastructure.Documents;

namespace TechShop.ECommerce.Infrastructure;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddDocumentServices(this IServiceCollection services)
    {
        services.AddScoped<IInvoicePdfGenerator, InvoicePdfGenerator>();
        return services;
    }
}