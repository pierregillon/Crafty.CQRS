using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Crafty.CQRS;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> where TCommand : ICommand
{
    Task IRequestHandler<TCommand>.Handle(TCommand request, CancellationToken _) => Handle(request);

    Task Handle(TCommand command);
}

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    Task<TResponse> IRequestHandler<TCommand, TResponse>.Handle(TCommand request, CancellationToken _) =>
        Handle(request);

    Task<TResponse> Handle(TCommand command);
}

public interface ICommandCancellableHandler<in TCommand> : IRequestHandler<TCommand> where TCommand : ICommand
{
    Task IRequestHandler<TCommand>.Handle(TCommand request, CancellationToken token) => Handle(request, token);

    new Task Handle(TCommand command, CancellationToken token);
}