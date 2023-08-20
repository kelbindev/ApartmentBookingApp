using Application.Behaviours;
using Domain.Booking;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependecyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Configuation =>
        {
            Configuation.RegisterServicesFromAssemblies(typeof(DependecyInjection).Assembly);
            Configuation.AddOpenBehavior(typeof(LoggingBehaviour<,>));
            Configuation.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });
        services.AddValidatorsFromAssembly(typeof(DependecyInjection).Assembly);
        services.AddTransient<PricingService>();

        return services;
    }
}
