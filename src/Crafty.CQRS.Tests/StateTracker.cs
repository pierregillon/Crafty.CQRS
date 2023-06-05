namespace Crafty.CQRS.Tests;

public record StateTracker
{
    public object? PreProcessed { get; set; }
    public object? PostProcessed { get; set; }
    public object? PostProcessResult { get; set; }
    public object? PipelineTriggeredRequest { get; set; }
    public object? DecoratedCommand { get; set; }
    public bool IsDecorationStarted { get; set; }
    public bool IsDecorationEnded { get; set; }
}