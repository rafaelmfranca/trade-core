namespace Application.Contracts.External;

public record MessageMetadata
{
    public string? MessageId { get; init; }
    public DateTime? Timestamp { get; init; }
    public string? Source { get; init; }
}