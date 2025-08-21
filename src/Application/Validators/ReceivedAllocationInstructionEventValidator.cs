using Application.Contracts.External;
using FluentValidation;

namespace Application.Validators;

public class ReceivedAllocationInstructionEventValidator : AbstractValidator<ReceivedAllocationInstructionEvent>
{
    public ReceivedAllocationInstructionEventValidator()
    {
        RuleFor(x => x.Message.OrderId)
            .NotEmpty()
            .WithMessage("OrderId is required");

        RuleFor(x => x.Message.TradeDate)
            .NotEqual(default(DateTime))
            .WithMessage("TradeDate must be valid");

        RuleFor(x => x.Message.Allocations)
            .NotEmpty()
            .WithMessage("At least one allocation is required");

        RuleForEach(x => x.Message.Allocations)
            .SetValidator(new AllocationDataValidator());
    }
}

public class AllocationDataValidator : AbstractValidator<AllocationData>
{
    public AllocationDataValidator()
    {
        RuleFor(x => x.AllocAccount)
            .NotEmpty()
            .WithMessage("AllocAccount is required");

        RuleFor(x => x.AllocQty)
            .GreaterThan(0)
            .WithMessage("AllocQty must be greater than 0");
    }
}