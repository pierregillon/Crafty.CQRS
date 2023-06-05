using Microsoft.Extensions.DependencyInjection;

namespace Crafty.CQRS.Tests;

public abstract class TestBase
{
    protected readonly ICommandDispatcher CommandDispatcher;
    protected readonly StateTracker StateTracker;
    protected readonly IQueryDispatcher QueryDispatcher;

    protected TestBase()
    {
        var serviceProvider = new ServiceCollection()
            .AddCqrs(options => options.RegisterServicesFromAssemblyContaining<TestBase>())
            .AddSingleton<StateTracker>()
            .BuildServiceProvider();

        CommandDispatcher = serviceProvider.GetRequiredService<ICommandDispatcher>();
        QueryDispatcher = serviceProvider.GetRequiredService<IQueryDispatcher>();
        StateTracker = serviceProvider.GetRequiredService<StateTracker>();
    }
}