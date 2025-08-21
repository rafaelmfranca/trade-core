namespace Application.Contracts.External;

public record AllocationInstructionMessage
{
    public string OrderId { get; init; } = string.Empty;
    public DateTime TradeDate { get; init; }
    public ICollection<AllocationData> Allocations { get; init; } = [];
    public DateTime AllocationTime { get; init; }
}

public record AllocationData
{
    public string AllocAccount { get; init; } = string.Empty;
    public decimal AllocQty { get; init; }
    public string AllocType { get; init; } = string.Empty;
}