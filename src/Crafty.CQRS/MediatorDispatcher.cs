using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Crafty.CQRS;

internal class MediatorDispatcher : ICommandDispatcher, IQueryDispatcher
{
    private readonly IMediator _mediator;

    public MediatorDispatcher(IMediator mediator) => _mediator = mediator;

    public Task Dispatch<TCommand>(TCommand command, CancellationToken token = default) where TCommand : ICommand =>
        _mediator.Send(command, token);

    public Task<TResult> Dispatch<TResult>(ICommand<TResult> command, CancellationToken token = default) =>
        _mediator.Send(command, token);

    public Task<TResult> Dispatch<TResult>(IQuery<TResult> query, CancellationToken token = default) =>
        _mediator.Send(query, token);
}