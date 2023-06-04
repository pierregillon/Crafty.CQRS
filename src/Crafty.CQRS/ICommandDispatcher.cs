using System.Threading;
using System.Threading.Tasks;

namespace Crafty.CQRS;

public interface ICommandDispatcher
{
    Task Dispatch<TCommand>(TCommand command, CancellationToken token = default) where TCommand : ICommand;
    Task<TResult> Dispatch<TResult>(ICommand<TResult> command, CancellationToken token = default);
}