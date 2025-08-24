using Core.Models;

namespace Core.Extensions;

public static class TradeIdExtensions
{
    public static TradeId ToTradeId(this ExecutionReport executionReport)
        => new(executionReport.TradeDate, executionReport.OrderId);

    public static TradeId ToTradeId(this AllocationInstruction allocInstruction)
        => new(allocInstruction.TradeDate, allocInstruction.OrderId);
    
    public static TradeId ToTradeId(this PartialTrade partialTrade)
        => new(partialTrade.TradeDate, partialTrade.OrderId);
}