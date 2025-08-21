namespace Application.Mappers;

public interface IMessageMapper<in TExternal, out TDomain>
{
    TDomain Map(TExternal @event);
}