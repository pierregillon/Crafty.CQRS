using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Crafty.CQRS.Tests;

public class QueryProcessorTests : TestBase
{
    [Fact]
    public async Task Query_is_correctly_preprocessed()
    {
        var query = new GetUserDetails();
        
        _ = await QueryDispatcher.Dispatch(query);

        StateTracker.PreProcessed.Should().Be(query);
    }
    
    [Fact]
    public async Task Query_is_correctly_postprocessed()
    {
        var query = new GetUserDetails();
        
        _ = await QueryDispatcher.Dispatch(query);

        StateTracker.PostProcessed.Should().Be(query);
    }
    
    [Fact]
    public async Task Post_processing_gets_the_same_result_the_handler_built()
    {
        var query = new GetUserDetails();
        
        var result = await QueryDispatcher.Dispatch(query);

        StateTracker.PostProcessResult.Should().Be(result);
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
                Tracker.PreProcessed = command;
                return Task.CompletedTask;
            }

            public Task PostProcess(GetUserDetails command, UserDetails result)
            {
                Tracker.PostProcessed = command;
                Tracker.PostProcessResult = result;
                return Task.CompletedTask;
            }
        }
    }
}