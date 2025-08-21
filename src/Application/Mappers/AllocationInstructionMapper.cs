using Application.Contracts.External;
using Core.Models;

namespace Application.Mappers;

public class AllocationInstructionMapper : IMessageMapper<ReceivedAllocationInstructionEvent, AllocationInstruction>
{
    public AllocationInstruction Map(ReceivedAllocationInstructionEvent @event)
    {
        var allocations = @event.Message.Allocations
            .Select(a => new Allocation
            {
                AllocAccount = a.AllocAccount,
                AllocQty = a.AllocQty,
                AllocType = a.AllocType
            })
            .ToList();

        return new AllocationInstruction
        {
            OrderId = @event.Message.OrderId,
            TradeDate = @event.Message.TradeDate,
            Allocations = allocations,
            AllocationTime = @event.Message.AllocationTime
        };
    }
}