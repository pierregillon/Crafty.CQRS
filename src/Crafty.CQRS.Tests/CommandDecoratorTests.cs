using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Crafty.CQRS.Tests;

public class CommandDecoratorTests : TestBase
{
    public CommandDecoratorTests() : base(ConfigureServices)
    {
    }

    private static IServiceCollection ConfigureServices(IServiceCollection services)
    {
        services
            .DecorateCommand<RegisterUser, RegisterUser.Decorator>()
            .DecorateAllCommands(typeof(LogCommandExecution<>));

        return services;
    }

    [Fact]
    public async Task Command_is_correctly_decorated()
    {
        var registerUser = new RegisterUser();

        await CommandDispatcher.Dispatch(registerUser);

        StateTracker.SpecificDecoratedCommand.Should().Be(registerUser);
    }

    [Fact]
    public async Task All_commands_can_be_decorated_with_generic_decorator()
    {
        var registerUser = new RegisterUser();

        await CommandDispatcher.Dispatch(registerUser);

        StateTracker.GenericDecoratedCommand.Should().Be(registerUser);
    }

    public record RegisterUser : ICommand
    {
        public record RegisterUserHandler : ICommandHandler<RegisterUser>
        {
            public Task Handle(RegisterUser command) => Task.CompletedTask;
        }

        public record Decorator(StateTracker Tracker) : ICommandDecorator<RegisterUser>
        {
            public async Task Decorate(RegisterUser command, CommandHandlerDelegate innerHandler)
            {
                Tracker.SpecificDecoratedCommand = command;
                await innerHandler();
            }
        }
    }

    public class LogCommandExecution<TCommand> : ICommandDecorator<TCommand> where TCommand : ICommand
    {
        public Task Decorate(TCommand command, CommandHandlerDelegate innerHandler) => Task.CompletedTask;
    }
}