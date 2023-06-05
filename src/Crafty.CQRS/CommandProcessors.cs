using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.Pipeline;

namespace Crafty.CQRS;

public interface ICommandPreProcessor<in TCommand> : IRequestPreProcessor<TCommand> where TCommand : ICommand
{
    Task IRequestPreProcessor<TCommand>.Process(TCommand command, CancellationToken _)
    {
        return PreProcess(command);
    }

    Task PreProcess(TCommand command);
}

public interface ICommandPostProcessor<in TCommand> : IRequestPostProcessor<TCommand, Unit> where TCommand : ICommand
{
    Task IRequestPostProcessor<TCommand, Unit>.Process(TCommand command, Unit unit, CancellationToken _)
    {
        return PostProcess(command);
    }

    Task PostProcess(TCommand command);
}

public interface ICommandPreProcessor<in TCommand, TResult> : IRequestPreProcessor<TCommand> where TCommand : ICommand<TResult>
{
    Task IRequestPreProcessor<TCommand>.Process(TCommand command, CancellationToken _)
    {
        return PreProcess(command);
    }

    Task PreProcess(TCommand command);
}

public interface ICommandPostProcessor<in TCommand, in TResult> : IRequestPostProcessor<TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task IRequestPostProcessor<TCommand, TResult>.Process(TCommand command, TResult result, CancellationToken _)
    {
        return PostProcess(command, result);
    }

    Task PostProcess(TCommand command, TResult result);
}