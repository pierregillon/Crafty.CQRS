using System;
using Microsoft.Extensions.DependencyInjection;

namespace Crafty.CQRS;

public static class DependencyInjection
{
    public static IServiceCollection AddCqrs(
        this IServiceCollection services,
        Action<MediatRServiceConfiguration> mediatorConfiguration
    )
    {
        services.AddMediatR(mediatorConfiguration);
        services.AddTransient<ICommandDispatcher, MediatorDispatcher>();
        services.AddTransient<IQueryDispatcher, MediatorDispatcher>();

        return services;
    }
}