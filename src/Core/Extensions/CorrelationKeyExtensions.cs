using Core.Models;

namespace Core.Extensions;

public static class CorrelationKeyExtensions
{
    public static CorrelationKey ToCorrelationKey(this ExecutionReport executionReport) 
        => new(executionReport.TradeDate, executionReport.OrderId);
    
    public static CorrelationKey ToCorrelationKey(this AllocationInstruction allocInstruction) 
        => new(allocInstruction.TradeDate, allocInstruction.OrderId);
}