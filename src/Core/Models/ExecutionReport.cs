namespace Core.Models;

public record ExecutionReport
{
    public string OrderId { get; init; } = string.Empty;
    public DateTime TradeDate { get; init; }
    public string Symbol { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
    public decimal Price { get; init; }
    public DateTime ExecutionTime { get; init; }
    public string Side { get; init; } = string.Empty;
    public string ExecutionType { get; init; } = string.Empty;
}