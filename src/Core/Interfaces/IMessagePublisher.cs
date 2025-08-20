using Core.Models;

namespace Core.Interfaces;

public interface IMessagePublisher
{
    Task PublishTradeAsync(Trade trade, CancellationToken cancellationToken = default);
    Task PublishToDeadLetterAsync<T>(T message, string reason, CancellationToken cancellationToken = default);
}