using Core.Models;

namespace Core.Interfaces;

public interface ICorrelationEngine
{
    Task<Trade?> ProcessExecutionReport(ExecutionReport executionReport, string routingKey);
    Task<Trade?> ProcessAllocationInstruction(AllocationInstruction allocationInstruction, string routingKey);
    
    Task<Trade> ForceAllocation(string correlationKey, Allocation allocation);
    Task<ICollection<PartialTrade>> GetOrphanedTrades(TimeSpan maxAge);
}