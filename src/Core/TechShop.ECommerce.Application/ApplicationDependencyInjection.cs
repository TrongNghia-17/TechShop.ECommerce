using TechShop.ECommerce.Application.Behaviors;
using TechShop.ECommerce.Application.Features.Orders.Shared;
using TechShop.ECommerce.Application.Features.Products.GetProductDetails;

namespace TechShop.ECommerce.Application;

public static class ApplicationDependencyInjection
{
    public static IServiceCollection AddCoreApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(
            _ => { },
            typeof(OrdersMappingProfile).Assembly);

        services.AddValidatorsFromAssemblyContaining<GetProductDetailsQuery>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(CachingBehavior<,>));
        });

        return services;
    }
}
