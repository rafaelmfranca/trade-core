using Core.Models;

namespace Core.Extensions;

public static class AllocationInstructionExtensions
{
    public static PartialTrade ToPartialTrade(
        this AllocationInstruction allocation,
        string routingKey,
        PartialTrade? existing = null)
        => new()
        {
            CorrelationKey = allocation.GetCorrelationKey(),
            OrderId = allocation.OrderId,
            TradeDate = allocation.TradeDate,
            Execution = existing?.Execution,
            Allocation = allocation,
            CreatedTime = existing?.CreatedTime ?? DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow,
            RoutingKey = routingKey
        };

    public static Trade ToTrade(
        this AllocationInstruction allocation,
        ExecutionReport execution,
        string routingKey,
        bool isForced = false)
        => new()
        {
            TradeId = allocation.OrderId,
            OrderId = allocation.OrderId,
            TradeDate = allocation.TradeDate,
            Execution = execution,
            Allocation = allocation,
            AssemblyTime = DateTime.UtcNow,
            IsForced = isForced,
            RoutingKey = routingKey
        };

    public static string GetCorrelationKey(this AllocationInstruction allocationInstruction)
        => $"{allocationInstruction.OrderId}:{allocationInstruction.TradeDate:yyyyMMdd}";
}