using Core.Models;

namespace Core.Extensions;

public static class ExecutionReportExtensions
{
    public static PartialTrade ToPartialTrade(
        this ExecutionReport execution,
        string routingKey,
        PartialTrade? existing = null)
        => new()
        {
            CorrelationKey = execution.GetCorrelationKey(),
            OrderId = execution.OrderId,
            TradeDate = execution.TradeDate,
            Execution = execution,
            Allocation = existing?.Allocation,
            CreatedTime = existing?.CreatedTime ?? DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow,
            RoutingKey = routingKey
        };

    public static Trade ToTrade(
        this ExecutionReport execution,
        AllocationInstruction allocation,
        string routingKey,
        bool isForced = false)
        => new()
        {
            TradeId = execution.OrderId,
            OrderId = execution.OrderId,
            TradeDate = execution.TradeDate,
            Execution = execution,
            Allocation = allocation,
            AssemblyTime = DateTime.UtcNow,
            IsForced = isForced,
            RoutingKey = routingKey
        };

    public static string GetCorrelationKey(this ExecutionReport execution)
        => $"{execution.OrderId}:{execution.TradeDate:yyyy-MM-dd}";
}