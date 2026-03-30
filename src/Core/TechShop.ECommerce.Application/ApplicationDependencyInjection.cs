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

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TracingPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            config.AddOpenBehavior(typeof(CachingBehavior<,>));
        });

        return services;
    }
}
