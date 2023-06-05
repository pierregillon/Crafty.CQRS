namespace Crafty.CQRS.Tests;

public record StateTracker
{
    public object? PreProcessed { get; set; }
    public object? PostProcessed { get; set; }
    public object? PostProcessResult { get; set; }
}