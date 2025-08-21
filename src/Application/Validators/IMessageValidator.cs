namespace Application.Validators;

public interface IMessageValidator<in T>
{
    bool IsValid(T message);
    ICollection<string> GetValidationErrors(T message);
}