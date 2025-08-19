namespace Core.Models;

public record AllocationInstruction
{
    public string OrderId { get; init; } = string.Empty;
    public DateTime TradeDate { get; init; }
    public ICollection<Allocation> Allocations { get; init; } = [];
    public DateTime AllocationTime { get; init; }
}

public record Allocation
{
    public string AllocAccount { get; init; } = string.Empty;
    public decimal AllocQty { get; init; }
    public string AllocType { get; init; } = string.Empty;
    public string? Reason { get; init; }
}