using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.Pipeline;

namespace Crafty.CQRS;

public interface IQueryPreProcessor<in TQuery, TResult> : IRequestPreProcessor<TQuery> where TQuery : IQuery<TResult>
{
    Task IRequestPreProcessor<TQuery>.Process(TQuery command, CancellationToken _)
    {
        return PreProcess(command);
    }

    Task PreProcess(TQuery command);
}

public interface IQueryPostProcessor<in TQuery, in TResult> : IRequestPostProcessor<TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task IRequestPostProcessor<TQuery, TResult>.Process(TQuery command, TResult result, CancellationToken _)
    {
        return PostProcess(command, result);
    }

    Task PostProcess(TQuery command, TResult result);
}