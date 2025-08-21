using Application.Contracts.External;
using FluentValidation;

namespace Application.Validators;

public class ReceivedExecutionReportEventValidator : AbstractValidator<ReceivedExecutionReportEvent>
{
    public ReceivedExecutionReportEventValidator()
    {
        RuleFor(x => x.Message.OrderId)
            .NotEmpty()
            .WithMessage("OrderId is required");

        RuleFor(x => x.Message.TradeDate)
            .NotEqual(default(DateTime))
            .WithMessage("TradeDate must be valid");
    }
}