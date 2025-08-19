using Core.Extensions;
using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.Logging;

namespace Core.Services;

public class CorrelationEngine(IStateStore stateStore, ILogger<CorrelationEngine> logger) : ICorrelationEngine
{
    public async Task<Trade?> ProcessExecutionReport(ExecutionReport executionReport, string routingKey)
    {
        var correlationKey = executionReport.GetCorrelationKey();
        var existingPartial = await stateStore.GetPartialTradeAsync(correlationKey);

        if (existingPartial?.Allocation is not null)
        {
            var trade = executionReport.ToTrade(existingPartial.Allocation, routingKey);
            await stateStore.SaveCompletedTradeAsync(trade);
            logger.LogInformation("Trade completed: {TradeId}", trade.TradeId);
            return trade;
        }

        var partial = executionReport.ToPartialTrade(routingKey, existingPartial);
        await stateStore.SavePartialTradeAsync(partial);
        logger.LogInformation("Execution saved as partial trade: {CorrelationKey}", correlationKey);
        return null;
    }

    public async Task<Trade?> ProcessAllocationInstruction(
        AllocationInstruction allocationInstruction,
        string routingKey)
    {
        var correlationKey = allocationInstruction.GetCorrelationKey();
        var existingPartial = await stateStore.GetPartialTradeAsync(correlationKey);

        if (existingPartial?.Execution is not null)
        {
            var trade = allocationInstruction.ToTrade(existingPartial.Execution, routingKey);
            await stateStore.SaveCompletedTradeAsync(trade);
            logger.LogInformation("Trade completed: {TradeId}", trade.TradeId);
            return trade;
        }

        var partial = allocationInstruction.ToPartialTrade(routingKey, existingPartial);
        await stateStore.SavePartialTradeAsync(partial);
        logger.LogInformation("Allocation saved as partial trade: {CorrelationKey}", correlationKey);
        return null;
    }

    public async Task<Trade> ForceAllocation(string correlationKey, Allocation allocation)
    {
        var partial = await stateStore.GetPartialTradeAsync(correlationKey);

        if (partial?.Execution is null)
            throw new InvalidOperationException($"Cannot force allocation for {correlationKey}: no execution found");

        var trade = partial.ToForcedTrade(allocation);
        await stateStore.SaveCompletedTradeAsync(trade);
        logger.LogInformation("Forced allocation created for trade: {TradeId}", trade.TradeId);
        return trade;
    }

    public async Task<ICollection<PartialTrade>> GetOrphanedTrades(TimeSpan maxAge)
        => await stateStore.GetPartialTradesByAgeAsync(maxAge);
}