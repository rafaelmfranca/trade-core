namespace Core.Models;

public record PartialTrade
{
    public string CorrelationKey { get; init; } = string.Empty;
    public string OrderId { get; init; } = string.Empty;
    public DateTime TradeDate { get; init; }
    public ExecutionReport? Execution { get; init; }
    public AllocationInstruction? Allocation { get; init; }
    public DateTime CreatedTime { get; init; }
    public DateTime LastUpdated { get; init; }
    public string RoutingKey { get; init; } = string.Empty;

    public bool IsComplete => Execution is not null && Allocation is not null;
    public TimeSpan Age => DateTime.UtcNow - CreatedTime;
}