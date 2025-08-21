namespace Application.Contracts.External;

public record ReceivedExecutionReportEvent
{
    public ExecutionReportMessage Message { get; init; } = new();
    public MessageMetadata? Metadata { get; init; }

    public string RoutingKey { get; init; } = string.Empty;
}