using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDTO>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage(Messages.PASSWORD_REQUIRED)
                .MinimumLength(6).WithMessage(Messages.PASSWORD_LENGTH);

            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage(Messages.PASSWORD_REQUIRED)
                .MinimumLength(6).WithMessage(Messages.PASSWORD_LENGTH);
        }
    }
}
