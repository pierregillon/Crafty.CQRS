using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Crafty.CQRS;

public interface ICommandDecorator<in TCommand> : IPipelineBehavior<TCommand, Unit> where TCommand : ICommand
{
    async Task<Unit> IPipelineBehavior<TCommand, Unit>.Handle(TCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
    {
        await Decorate(request, async () => _ = await next());

        return Unit.Value;
    }

    Task Decorate(TCommand command, CommandHandlerDelegate innerHandler);
}

public delegate Task CommandHandlerDelegate();