using FluentAssertions;

namespace Crafty.CQRS.Tests;

public class CommandProcessorTests : TestBase
{
    [Fact]
    public async Task Command_is_correctly_preprocessed()
    {
        var command = new RegisterUser();
        
        await CommandDispatcher.Dispatch(command);

        StateTracker.PreProcessed.Should().Be(command);
    }
    
    [Fact]
    public async Task Command_is_correctly_postprocessed()
    {
        var command = new RegisterUser();
        
        await CommandDispatcher.Dispatch(command);

        StateTracker.PostProcessed.Should().Be(command);
    }
    
    [Fact]
    public async Task Command_with_result_is_correctly_preprocessed()
    {
        var command = new RegisterUserWithResult();
        
        _ = await CommandDispatcher.Dispatch(command);

        StateTracker.PreProcessed.Should().Be(command);
    }
    
    [Fact]
    public async Task Command_with_result_is_correctly_postprocessed()
    {
        var command = new RegisterUserWithResult();
        
        _ = await CommandDispatcher.Dispatch(command);

        StateTracker.PostProcessed.Should().Be(command);
    }
    
    [Fact]
    public async Task Post_processing_gets_the_same_result_the_handler_built()
    {
        var command = new RegisterUserWithResult();
        
        var result = await CommandDispatcher.Dispatch(command);

        StateTracker.PostProcessResult.Should().Be(result);
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
                Tracker.PreProcessed = command;
                return Task.CompletedTask;
            }

            public Task PostProcess(RegisterUser command)
            {
                Tracker.PostProcessed = command;
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
                Tracker.PreProcessed = command;
                return Task.CompletedTask;
            }

            public Task PostProcess(RegisterUserWithResult command, Guid guid)
            {
                Tracker.PostProcessed = command;
                Tracker.PostProcessResult = guid;
                return Task.CompletedTask;
            }
        }
    }
}