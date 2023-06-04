using System.Threading;
using System.Threading.Tasks;

namespace Crafty.CQRS;

public interface IQueryDispatcher
{
    Task<TResult> Dispatch<TResult>(IQuery<TResult> query, CancellationToken token = default);
}