using System;
using MediatR;
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

    public static IServiceCollection DecorateCommand<TCommand, TCommandDecorator>(this IServiceCollection services)
        where TCommand : ICommand
        where TCommandDecorator : class, ICommandDecorator<TCommand>
    {
        services.AddTransient<IPipelineBehavior<TCommand, Unit>, TCommandDecorator>();

        return services;
    }
}