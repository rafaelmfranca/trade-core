namespace Application.Contracts.External;

public record ReceivedAllocationInstructionEvent
{
    public AllocationInstructionMessage Message { get; init; } = new();
    public MessageMetadata? Metadata { get; init; }

    public string RoutingKey { get; init; } = string.Empty;
}