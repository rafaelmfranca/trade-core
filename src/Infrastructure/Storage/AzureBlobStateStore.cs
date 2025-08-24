using System.Collections.Concurrent;
using System.Text.Json;
using Azure.Storage.Blobs;
using Core.Extensions;
using Core.Interfaces;
using Core.Models;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Storage;

public class AzureBlobStateStore(
    BlobContainerClient containerClient,
    BlobStorageOptions storageOptions,
    ILogger<AzureBlobStateStore> logger) : IStateStore
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };
    private readonly ConcurrentDictionary<CorrelationKey, PartialTrade> _cache = new();
    
    public async Task<PartialTrade?> GetPartialTradeAsync(CorrelationKey correlationKey)
    {
        if (_cache.TryGetValue(correlationKey, out var cachedPartialTrade))
            return cachedPartialTrade;

        try
        {
            var blobName = GetPartialTradeBlobName(correlationKey);
            var blobClient = containerClient.GetBlobClient(blobName);

            if (!await blobClient.ExistsAsync())
                return null;

            var response = await blobClient.DownloadContentAsync();
            var partialTrade = JsonSerializer.Deserialize<PartialTrade>(response.Value.Content.ToString());

            if (partialTrade is not null)
                _cache.TryAdd(correlationKey, partialTrade);

            return partialTrade;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving partial trade: {CorrelationKey}", correlationKey.ToString());
            throw;
        }
    }

    public Task<ICollection<PartialTrade>> GetPartialTradesAsync(DateTime? from = null, DateTime? to = null)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<PartialTrade>> GetPartialTradesByAgeAsync(TimeSpan age)
    {
        throw new NotImplementedException();
    }

    public Task<Trade?> GetTradeAsync(TradeId tradeId)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<Trade>> GetCompletedTradesAsync(DateTime? from = null, DateTime? to = null)
    {
        throw new NotImplementedException();
    }

    public async Task SavePartialTradeAsync(PartialTrade partial)
    {
        try
        {
            var blobName = GetPartialTradeBlobName(partial.CorrelationKey);
            var blobClient = containerClient.GetBlobClient(blobName);

            var json = JsonSerializer.Serialize(partial, _jsonSerializerOptions);
            await blobClient.UploadAsync(BinaryData.FromString(json), overwrite: true);

            _cache.AddOrUpdate(partial.CorrelationKey, partial, (_, _) => partial);
            logger.LogDebug("Saved partial trade: {CorrelationKey}", partial.CorrelationKey.ToString());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error saving partial trade: {CorrelationKey}", partial.CorrelationKey.ToString());
            throw;
        }
    }

    public async Task SaveCompletedTradeAsync(Trade trade)
    {
        try
        {
            var blobName = GetCompletedTradeBlobName(trade.TradeId);
            var blobClient = containerClient.GetBlobClient(blobName);

            var json = JsonSerializer.Serialize(trade, _jsonSerializerOptions);
            await blobClient.UploadAsync(BinaryData.FromString(json), overwrite: true);
            
            // TODO: Refactor this!
            var correlationKey = trade.TradeId.ToCorrelationKey();
            _cache.TryRemove(correlationKey, out _);

            var partialBlobName = GetPartialTradeBlobName(correlationKey);
            var partialBlobClient = containerClient.GetBlobClient(partialBlobName);
            await partialBlobClient.DeleteIfExistsAsync();
            
            logger.LogDebug("Saved completed trade: {TradeId}", trade.TradeId.ToString());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error saving completed trade: {TradeId}", trade.TradeId);
            throw;
        }
    }

    public Task ClearInMemoryCache()
    {
        _cache.Clear();
        logger.LogInformation("In-memory cache cleared.");
        return Task.CompletedTask;
    }

    private string GetPartialTradeBlobName(CorrelationKey correlationKey)
        => $"{correlationKey.ReferenceDate:yyyyMMdd}/{storageOptions.PartialTradesPrefix}/{correlationKey.Key}.json";
    
    private string GetCompletedTradeBlobName(TradeId tradeId)
        => $"{tradeId.TradeDate:yyyyMMdd}/{storageOptions.CompletedTradesPrefix}/{tradeId.Id}.json";
}