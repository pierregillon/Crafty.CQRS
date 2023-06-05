namespace Crafty.CQRS.Tests;

public record StateTracker
{
    public object? PreProcessed { get; set; }
    public object? PostProcessed { get; set; }
    public object? PostProcessResult { get; set; }
    public object? PipelineTriggeredRequest { get; set; }
    public object? SpecificDecoratedCommand { get; set; }
    public object? GenericDecoratedCommand { get; set; }
}