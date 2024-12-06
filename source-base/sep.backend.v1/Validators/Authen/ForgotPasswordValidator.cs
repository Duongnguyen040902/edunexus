using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Requests.Auth;

namespace sep.backend.v1.Validators
{
    public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.EMAIL_REQUIRED)
                .EmailAddress().WithMessage(Messages.EMAIL_INVALID);
        }
    }
}
