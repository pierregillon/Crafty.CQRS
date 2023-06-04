using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Crafty.CQRS.Tests;

public class QueryDispatcherTests
{
    private readonly IQueryDispatcher _queryDispatcher;

    public QueryDispatcherTests()
    {
        var serviceProvider = new ServiceCollection()
            .AddCqrs(options => options.RegisterServicesFromAssemblyContaining<QueryDispatcherTests>())
            .BuildServiceProvider();

        _queryDispatcher = serviceProvider.GetRequiredService<IQueryDispatcher>();
    }

    [Fact]
    public async Task Dispatching_query_resolves_handler_based_on_query_type()
    {
        var results = await _queryDispatcher.Dispatch(new GetAllProducts());

        results.Should().BeEquivalentTo(new[]
        {
            new GetAllProducts.ProductListItem("A"),
            new GetAllProducts.ProductListItem("B")
        });
    }

    public record GetAllProducts : IQuery<IEnumerable<GetAllProducts.ProductListItem>>
    {
        public record ProductListItem(string Name);

        public record GetAllProductsHandler : IQueryHandler<GetAllProducts, IEnumerable<ProductListItem>>
        {
            public Task<IEnumerable<ProductListItem>> Handle(GetAllProducts query) => Task.FromResult(new[]
            {
                new ProductListItem("A"),
                new ProductListItem("B")
            }.AsEnumerable());
        }
    }
}