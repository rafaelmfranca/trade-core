namespace Core.Models;

public readonly record struct TradeId(DateTime TradeDate, string Id)
{
    public override string ToString()
        => $"{TradeDate:yyyyMMdd}:{Id}";
}