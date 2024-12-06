using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Requests.Auth;

namespace sep.backend.v1.Validators
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordValidator() {
            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.PASSWORD_REQUIRED)
                .MinimumLength(6).WithMessage(Messages.PASSWORD_LENGTH);

            RuleFor(x => x.ConfirmPassword)
                .Cascade(CascadeMode.Stop)
                .Equal(x => x.Password).WithMessage(Messages.PASSWORD_MATCH);
        }
    }
}
