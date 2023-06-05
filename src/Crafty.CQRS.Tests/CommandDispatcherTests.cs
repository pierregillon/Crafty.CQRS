using FluentAssertions;

namespace Crafty.CQRS.Tests;

public class CommandDispatcherTests : TestBase
{
    [Fact]
    public async Task Dispatching_command_resolves_handler_based_on_command_type()
    {
        var command = new CreateProduct();

        await CommandDispatcher.Dispatch(command);

        command.IsHandled.Should().BeTrue();
    }

    [Fact]
    public async Task Dispatching_command_with_result_resolves_handler_based_on_command_type()
    {
        var userId = await CommandDispatcher.Dispatch(new RegisterUser());

        userId.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Dispatching_cancellable_command_resolves_handler_based_on_command_type()
    {
        var cancellationTokenSource = new CancellationTokenSource(100);

        var action = () => CommandDispatcher.Dispatch(new CancellableCommand(), cancellationTokenSource.Token);

        await action
            .Should()
            .ThrowAsync<OperationCanceledException>();
    }

    public record CreateProduct : ICommand
    {
        public bool IsHandled { get; private set; }

        private Task Handled()
        {
            IsHandled = true;
            return Task.CompletedTask;
        }

        public record CreateProductHandler : ICommandHandler<CreateProduct>
        {
            public Task Handle(CreateProduct command) => command.Handled();
        }
    }

    public record RegisterUser : ICommand<Guid>
    {
        public record RegisterUserHandler : ICommandHandler<RegisterUser, Guid>
        {
            public Task<Guid> Handle(RegisterUser command) => Task.FromResult(Guid.NewGuid());
        }
    }

    public record CancellableCommand : ICommand
    {
        public record CancellableCommandHandler : ICommandCancellableHandler<CancellableCommand>
        {
            public Task Handle(CancellableCommand command, CancellationToken token)
            {
                while (true)
                {
                    Task.Delay(50, token);
                    token.ThrowIfCancellationRequested();
                }
            }
        }
    }
}