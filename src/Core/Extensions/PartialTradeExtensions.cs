using Core.Models;

namespace Core.Extensions;

public static class PartialTradeExtensions
{
    public static Trade ToCompletedTrade(this PartialTrade partial, bool isForced = false)
    {
        if (!partial.IsComplete)
            throw new InvalidOperationException($"Cannot create completed trade from incomplete partial: {partial.CorrelationKey}");

        return new Trade
        {
            TradeId = partial.OrderId,
            OrderId = partial.OrderId,
            TradeDate = partial.TradeDate,
            Execution = partial.Execution!,
            Allocation = partial.Allocation!,
            AssemblyTime = DateTime.UtcNow,
            IsForced = isForced,
            RoutingKey = partial.RoutingKey
        };
    }

    public static Trade ToForcedTrade(this PartialTrade partial, Allocation allocation)
    {
        if (partial.Execution is null)
            throw new InvalidOperationException($"Cannot force allocation for {partial.CorrelationKey}: no execution found");

        var forcedAllocation = new AllocationInstruction
        {
            OrderId = partial.OrderId,
            TradeDate = partial.TradeDate,
            Allocations = [allocation],
            AllocationTime = DateTime.UtcNow
        };

        return new Trade
        {
            TradeId = partial.OrderId,
            OrderId = partial.OrderId,
            TradeDate = partial.TradeDate,
            Execution = partial.Execution,
            Allocation = forcedAllocation,
            AssemblyTime = DateTime.UtcNow,
            IsForced = true,
            RoutingKey = partial.RoutingKey
        };
    }
}