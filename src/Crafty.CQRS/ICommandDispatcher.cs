using System.Threading;
using System.Threading.Tasks;

namespace Crafty.CQRS;

public interface ICommandDispatcher
{
    Task Dispatch<TCommand>(TCommand command) where TCommand : ICommand;
    Task Dispatch<TCommand>(TCommand command, CancellationToken token) where TCommand : ICommand;
    Task<TResult> Dispatch<TResult>(ICommand<TResult> command);
}