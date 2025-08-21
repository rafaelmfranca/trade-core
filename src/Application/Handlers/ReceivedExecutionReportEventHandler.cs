using Application.Contracts.External;
using Application.Mappers;
using Application.Validators;
using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.Logging;

namespace Application.Handlers;

public class ReceivedExecutionReportEventHandler(
    IMessagePublisher publisher,
    ICorrelationEngine correlationEngine,
    IMessageValidator<ReceivedExecutionReportEvent> validator,
    IMessageMapper<ReceivedExecutionReportEvent, ExecutionReport> mapper,
    ILogger<ReceivedExecutionReportEventHandler> logger)
{
    public async Task Handle(ReceivedExecutionReportEvent @event)
    {
        try
        {
            if (!validator.IsValid(@event))
            {
                var errors = validator.GetValidationErrors(@event);
                var errorMessage = string.Join(", ", errors);

                logger.LogWarning("Invalid {EventType} received: {Errors}", @event.Message.GetType(), errorMessage);
                await publisher.PublishToDeadLetterAsync(@event, $"Validation failed: {errorMessage}");
                return;
            }

            var executionReport = mapper.Map(@event);
            var trade = await correlationEngine.ProcessExecutionReport(executionReport, @event.RoutingKey);

            if (trade is null)
                return;

            await publisher.PublishTradeAsync(trade);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing {EventType}: {OrderId}", @event.Message.GetType(), @event.Message.OrderId);
            throw;
        }
    }
}