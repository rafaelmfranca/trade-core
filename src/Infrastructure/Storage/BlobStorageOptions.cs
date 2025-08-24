namespace Infrastructure.Storage;

public class BlobStorageOptions
{
    public const string SectionName = "AzureStorage";

    public string ConnectionString { get; init; } = string.Empty;
    public string ContainerName { get; init; } = "trade-core";
    public string PartialTradesPrefix { get; init; } = "partial-trades";
    public string CompletedTradesPrefix { get; init; } = "completed-trades";
}