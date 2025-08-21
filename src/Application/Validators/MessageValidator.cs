using FluentValidation;

namespace Application.Validators;

public class MessageValidator<T>(IValidator<T> validator) : IMessageValidator<T>
{
    public bool IsValid(T message)
        => validator.Validate(message).IsValid;

    public ICollection<string> GetValidationErrors(T message)
        => validator.Validate(message).Errors.Select(e => e.ErrorMessage).ToList();
}