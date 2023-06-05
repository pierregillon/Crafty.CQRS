using System;
using System.Linq;
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

    public static IServiceCollection DecorateAllCommands(this IServiceCollection services, Type type)
    {
        // var commands = Assembly
        //     .GetExecutingAssembly()
        //     .GetTypes()
        //     .Where(x => x.GetInterfaces().Contains(typeof(ICommand)))
        //     .ToArray();
        //
        // if (commands.Length == 0)
        // {
        //     throw new InvalidOperationException($"No command type found in {Assembly.GetExecutingAssembly().FullName}");
        // }

        var pipelineBehaviorType = typeof(ICommandDecorator<>).GetInterfaces().Single().GetGenericTypeDefinition();

        services.AddTransient(pipelineBehaviorType, type);

        // foreach (var commandType in commands)
        // {
        //     services.AddTransient(pipelineBehaviorType, type.MakeGenericType(commandType));
        // }

        return services;
    }
}