namespace Core.Interfaces;

public interface IIdempotencyService
{
    Task<bool> HasBeenProcessedAsync(string messageType, string messageId);
    Task MarkAsProcessedAsync(string messageType, string messageId);
}