using FluentValidation;
using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Extensions.EF;

namespace sep.backend.v1.Validators
{
    public class RegisterSchoolValidator : AbstractValidator<RegisterDTORemake>
    {
        private readonly ApplicationContext _context;

        public RegisterSchoolValidator(ApplicationContext context)
        {
            _context = context;

            RuleFor(x => x.Username)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.USERNAME_REQUIRED)
                .Length(2, 50).WithMessage(Messages.USERNAME_LENGTH)
                .Matches(@"^[a-zA-Z0-9]+$").WithMessage(Messages.USERNAME_LETTER)
                .Must(BeUniqueUsername).WithMessage(Messages.USERNAME_EXISTS);

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.NAME_REQUIRED)
                .Length(2, 50).WithMessage(Messages.NAME_LENGTH)
                .Matches(@"^[a-zA-Z\s]+$").WithMessage(Messages.NAME_LETTER);

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.EMAIL_REQUIRED)
                .EmailAddress().WithMessage(Messages.EMAIL_INVALID)
                .Must(BeUniqueEmail).WithMessage(Messages.EMAIL_EXISTS);

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.PASSWORD_REQUIRED)
                .MinimumLength(6).WithMessage(Messages.PASSWORD_LENGTH);

            RuleFor(x => x.RePassword)
                .Cascade(CascadeMode.Stop)
                .Equal(x => x.Password).WithMessage(Messages.PASSWORD_MATCH);
        }

        private bool BeUniqueEmail(string email)
        {
            return !_context.Schools.Any(u => u.Email == email);
        }

        private bool BeUniqueUsername(string username)
        {
            return !_context.Schools.Any(u => u.Username == username);
        }
    }
}