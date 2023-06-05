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
            .DecorateCommand<RegisterUser, RegisterUser.Decorator>();

        return services;
    }

    [Fact]
    public async Task Command_is_correctly_decorated()
    {
        var registerUser = new RegisterUser();

        await CommandDispatcher.Dispatch(registerUser);

        StateTracker.IsDecorationStarted.Should().BeTrue();
        StateTracker.IsDecorationEnded.Should().BeTrue();
        StateTracker.DecoratedCommand.Should().Be(registerUser);
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
                Tracker.DecoratedCommand = command;
                Tracker.IsDecorationStarted = true;
                await innerHandler();
                Tracker.IsDecorationEnded = true;
            }
        }
    }
}