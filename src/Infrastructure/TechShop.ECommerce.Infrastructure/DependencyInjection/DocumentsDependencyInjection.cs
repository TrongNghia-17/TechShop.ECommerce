using TechShop.ECommerce.Application.Contracts.Documents;
using TechShop.ECommerce.Infrastructure.Documents;

namespace TechShop.ECommerce.Infrastructure.DependencyInjection;

public static class DocumentsDependencyInjection
{
    public static IServiceCollection AddDocumentInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IInvoicePdfGenerator, InvoicePdfGenerator>();
        return services;
    }
}