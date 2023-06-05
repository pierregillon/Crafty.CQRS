using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Crafty.CQRS.Tests;

public class PipelineBehaviorCompatibility : TestBase
{
    public PipelineBehaviorCompatibility() : base(ConfigureServices())
    {
    }

    private static Func<IServiceCollection, IServiceCollection> ConfigureServices() => 
        services => services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LogAllRequests<,>));

    [Fact]
    public async Task Pipeline_behavior_correctly_triggered_on_command()
    {
        var command = new RegisterUser();
        
        await CommandDispatcher.Dispatch(command);
    
        StateTracker.PipelineTriggeredRequest.Should().Be(command);
    }
    
    [Fact]
    public async Task Pipeline_behavior_correctly_triggered_on_query()
    {
        var query = new GetUserId();
        
        await QueryDispatcher.Dispatch(query);
    
        StateTracker.PipelineTriggeredRequest.Should().Be(query);
    }
    
    public record RegisterUser : ICommand<Guid>
    {
        public record RegisterUserHandler : ICommandHandler<RegisterUser, Guid>
        {
            public Task<Guid> Handle(RegisterUser command) => Task.FromResult(Guid.NewGuid());
        }
    }
    
    public record GetUserId : IQuery<Guid>
    {
        public record RegisterUserHandler : IQueryHandler<GetUserId, Guid>
        {
            public Task<Guid> Handle(GetUserId query) => Task.FromResult(Guid.NewGuid());
        }
    }

    public record LogAllRequests<TRequest, TResponse>(StateTracker Tracker) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            Tracker.PipelineTriggeredRequest = request;
            return await next();
        }
    }
}