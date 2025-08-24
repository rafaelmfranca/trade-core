namespace Core.Models;

public readonly record struct CorrelationKey(DateTime ReferenceDate, string Key)
{
    public override string ToString()
        => $"{ReferenceDate:yyyyMMdd}:{Key}";
}