using Microsoft.Extensions.DependencyInjection;

namespace Crafty.CQRS.Tests;

public abstract class TestBase
{
    protected readonly ICommandDispatcher CommandDispatcher;
    protected readonly StateTracker StateTracker;
    protected readonly IQueryDispatcher QueryDispatcher;

    protected TestBase(Func<IServiceCollection, IServiceCollection>? configure = null)
    {
        IServiceCollection serviceCollection = new ServiceCollection()
            .AddCqrs(options => options.RegisterServicesFromAssemblyContaining<TestBase>())
            .AddSingleton<StateTracker>();

        if (configure is not null)
        {
            serviceCollection = configure(serviceCollection);
        }
        
        var serviceProvider = serviceCollection.BuildServiceProvider();

        CommandDispatcher = serviceProvider.GetRequiredService<ICommandDispatcher>();
        QueryDispatcher = serviceProvider.GetRequiredService<IQueryDispatcher>();
        StateTracker = serviceProvider.GetRequiredService<StateTracker>();
    }
}