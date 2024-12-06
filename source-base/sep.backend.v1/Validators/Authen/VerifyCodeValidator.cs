using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Requests.Auth;

namespace sep.backend.v1.Validators
{
    public class VerifyCodeValidator : AbstractValidator<VerifyEmailRequest>
    {
        public VerifyCodeValidator()
        {
            RuleFor(x => x.VerificationCode)
                .NotEmpty()
                .WithMessage(Messages.CONFIRM_CODE_REQUIRED);
        }
    }
}
