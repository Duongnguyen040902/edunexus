using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Validators
{
    public class TimeSlotValidator : AbstractValidator<CreateTimeSlotDTO>
    {
        public TimeSlotValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.NAME_REQUIRED)
                .Length(2, 50).WithMessage(Messages.NAME_LENGTH);

            RuleFor(x => x.StartTime)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.START_TIME_REQUIRED);

            RuleFor(x => x.EndTime)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.END_TIME_REQUIRED)
                .GreaterThan(x => x.StartTime).WithMessage(Messages.END_TIME_AFTER_START_TIME);

            RuleFor(x => x.IsActive)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(Messages.IS_ACTIVE_REQUIRED);
        }
    }
}
