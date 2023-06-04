using MediatR;

namespace Crafty.CQRS;

public interface IQuery<out TResult> : IRequest<TResult>
{
}