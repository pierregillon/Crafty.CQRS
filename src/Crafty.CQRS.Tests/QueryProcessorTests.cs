using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Crafty.CQRS.Tests;

public class QueryProcessorTests
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly StateTracker _stateTracker;

    public QueryProcessorTests()
    {
        var serviceProvider = new ServiceCollection()
            .AddCqrs(options => options.RegisterServicesFromAssemblyContaining<QueryProcessorTests>())
            .AddSingleton<StateTracker>()
            .BuildServiceProvider();

        _queryDispatcher = serviceProvider.GetRequiredService<IQueryDispatcher>();
        _stateTracker = serviceProvider.GetRequiredService<StateTracker>();
    }
    
    [Fact]
    public async Task Query_is_correctly_preprocessed()
    {
        var query = new GetUserDetails();
        
        _ = await _queryDispatcher.Dispatch(query);

        _stateTracker.PreProcessedQuery.Should().Be(query);
    }
    
    [Fact]
    public async Task Query_is_correctly_postprocessed()
    {
        var query = new GetUserDetails();
        
        _ = await _queryDispatcher.Dispatch(query);

        _stateTracker.PostProcessedQuery.Should().Be(query);
    }
    
    [Fact]
    public async Task Post_processing_gets_the_same_result_the_handler_built()
    {
        var query = new GetUserDetails();
        
        var result = await _queryDispatcher.Dispatch(query);

        _stateTracker.PostProcessResult.Should().Be(result);
    }

    public record StateTracker
    {
        public object? PreProcessedQuery { get; set; }
        public object? PostProcessedQuery { get; set; }
        public object? PostProcessResult { get; set; }
    }

    public record GetUserDetails : IQuery<GetUserDetails.UserDetails>
    {
        public record UserDetails(string Name);
        
        public record RegisterUserHandler : IQueryHandler<GetUserDetails, UserDetails>
        {
            public Task<UserDetails> Handle(GetUserDetails command) => Task.FromResult(new UserDetails("John Doe"));
        }
        
        public record Processor(StateTracker Tracker) : IQueryPreProcessor<GetUserDetails, UserDetails>, IQueryPostProcessor<GetUserDetails, UserDetails>
        {
            public Task PreProcess(GetUserDetails command)
            {
                Tracker.PreProcessedQuery = command;
                return Task.CompletedTask;
            }

            public Task PostProcess(GetUserDetails command, UserDetails result)
            {
                Tracker.PostProcessedQuery = command;
                Tracker.PostProcessResult = result;
                return Task.CompletedTask;
            }
        }
    }
}