using Application.Contracts.External;
using Core.Models;

namespace Application.Mappers;

public class ExecutionReportMapper : IMessageMapper<ReceivedExecutionReportEvent, ExecutionReport>
{
    public ExecutionReport Map(ReceivedExecutionReportEvent @event)
        => new()
        {
            OrderId = @event.Message.OrderId,
            TradeDate = @event.Message.TradeDate,
            Symbol = @event.Message.Symbol,
            Quantity = @event.Message.Quantity,
            Price = @event.Message.Price,
            ExecutionTime = @event.Message.ExecutionTime,
            Side = @event.Message.Side,
            ExecutionType = @event.Message.ExecutionType
        };
}