using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Crafty.CQRS;

internal class MediatorDispatcher : ICommandDispatcher, IQueryDispatcher
{
    private readonly IMediator _mediator;

    public MediatorDispatcher(IMediator mediator) => _mediator = mediator;

    public Task Dispatch<TCommand>(TCommand command) where TCommand : ICommand => _mediator.Send(command);

    public Task Dispatch<TCommand>(TCommand command, CancellationToken token) where TCommand : ICommand =>
        _mediator.Send(command, token);

    public Task<TResult> Dispatch<TResult>(ICommand<TResult> command) => _mediator.Send(command);

    public Task<TResult> Dispatch<TResult>(IQuery<TResult> query) => _mediator.Send(query);
}