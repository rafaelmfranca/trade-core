using Core.Models;

namespace Core.Interfaces;

public interface IStateStore
{
    Task<PartialTrade?> GetPartialTradeAsync(string correlationKey);
    Task<ICollection<PartialTrade>> GetPartialTradesAsync(DateTime? from = null, DateTime? to = null);
    Task<ICollection<PartialTrade>> GetPartialTradesByAgeAsync(TimeSpan age);

    Task<Trade?> GetTradeAsync(string tradeId);
    Task<ICollection<Trade>> GetCompletedTradesAsync(DateTime? from = null, DateTime? to = null);

    Task SavePartialTradeAsync(PartialTrade partial);
    Task SaveCompletedTradeAsync(Trade trade);

    Task ClearInMemoryCache();
}