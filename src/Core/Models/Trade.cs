namespace Core.Models;

public record Trade
{
    public TradeId TradeId { get; init; }
    public string OrderId { get; init; } = string.Empty;
    public DateTime TradeDate { get; init; }
    public ExecutionReport Execution { get; init; } = null!;
    public AllocationInstruction Allocation { get; init; } = null!;
    public DateTime AssemblyTime { get; init; }
    public bool IsForced { get; init; }
    public string RoutingKey { get; init; } = string.Empty;
}