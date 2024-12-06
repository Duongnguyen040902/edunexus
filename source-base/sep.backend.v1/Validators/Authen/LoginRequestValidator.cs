using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Helpers;
using sep.backend.v1.Requests.Auth;

namespace sep.backend.v1.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Messages.EMAIL_REQUIRED);

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Messages.PASSWORD_REQUIRED);

        RuleFor(x => x.Mode)
            .Cascade(CascadeMode.Stop)
            .Must(BeValidStatusAccount).WithMessage(StringHelper.FormatMessage(Messages.INVALID, "Vai trò đăng nhập"));
    }

    private bool BeValidStatusAccount(int mode)
    {
        return Enum.IsDefined(typeof(ModeLogin), mode);
    }
}