using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Crafty.CQRS.Tests;

public class CommandBehaviourTests
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly StateTracker _stateTracker;

    public CommandBehaviourTests()
    {
        var serviceProvider = new ServiceCollection()
            .AddCqrs(options => options.RegisterServicesFromAssemblyContaining<CommandBehaviourTests>())
            .AddSingleton<StateTracker>()
            .BuildServiceProvider();

        _commandDispatcher = serviceProvider.GetRequiredService<ICommandDispatcher>();
        _stateTracker = serviceProvider.GetRequiredService<StateTracker>();
    }
    
    [Fact]
    public async Task Command_is_correctly_preprocessed()
    {
        var command = new RegisterUser();
        
        await _commandDispatcher.Dispatch(command);

        _stateTracker.CommandPreProcessed.Should().Be(command);
    }
    
    [Fact]
    public async Task Command_is_correctly_postprocessed()
    {
        var command = new RegisterUser();
        
        await _commandDispatcher.Dispatch(command);

        _stateTracker.CommandPostProcessed.Should().Be(command);
    }
    
    [Fact]
    public async Task Command_with_result_is_correctly_preprocessed()
    {
        var command = new RegisterUserWithResult();
        
        _ = await _commandDispatcher.Dispatch(command);

        _stateTracker.CommandPreProcessed.Should().Be(command);
    }
    
    [Fact]
    public async Task Command_with_result_is_correctly_postprocessed()
    {
        var command = new RegisterUserWithResult();
        
        _ = await _commandDispatcher.Dispatch(command);

        _stateTracker.CommandPostProcessed.Should().Be(command);
    }
    
    [Fact]
    public async Task Post_processing_gets_the_same_result_the_handler_built()
    {
        var command = new RegisterUserWithResult();
        
        var result = await _commandDispatcher.Dispatch(command);

        _stateTracker.ResultFromPostProcessed.Should().Be(result);
    }

    public record StateTracker
    {
        public object? CommandPreProcessed { get; set; }
        public object? CommandPostProcessed { get; set; }
        public object? ResultFromPostProcessed { get; set; }
    }

    public record RegisterUser : ICommand
    {
        public record RegisterUserHandler : ICommandHandler<RegisterUser>
        {
            public Task Handle(RegisterUser command) => Task.CompletedTask;
        }
        
        public record Processor(StateTracker Tracker) : ICommandPreProcessor<RegisterUser>, ICommandPostProcessor<RegisterUser>
        {
            public Task PreProcess(RegisterUser command)
            {
                Tracker.CommandPreProcessed = command;
                return Task.CompletedTask;
            }

            public Task PostProcess(RegisterUser command)
            {
                Tracker.CommandPostProcessed = command;
                return Task.CompletedTask;
            }
        }
    }

    public record RegisterUserWithResult : ICommand<Guid>
    {
        public record RegisterUserHandler : ICommandHandler<RegisterUserWithResult, Guid>
        {
            public Task<Guid> Handle(RegisterUserWithResult command) => Task.FromResult(Guid.NewGuid());
        }
        
        public record Processor(StateTracker Tracker) : ICommandPreProcessor<RegisterUserWithResult, Guid>, ICommandPostProcessor<RegisterUserWithResult, Guid>
        {
            public Task PreProcess(RegisterUserWithResult command)
            {
                Tracker.CommandPreProcessed = command;
                return Task.CompletedTask;
            }

            public Task PostProcess(RegisterUserWithResult command, Guid guid)
            {
                Tracker.CommandPostProcessed = command;
                Tracker.ResultFromPostProcessed = guid;
                return Task.CompletedTask;
            }
        }
    }
}