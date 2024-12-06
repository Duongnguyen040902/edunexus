using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.DTOs;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Helpers;

namespace sep.backend.v1.Validators
{
    public class CreateSchoolValidator : AbstractValidator<CreateSchoolDTO>
    {
        private readonly ApplicationContext _context;
        public CreateSchoolValidator(ApplicationContext context)
        {
            _context = context;

            RuleFor(x => x.Username)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.USERNAME_REQUIRED)
                .Length(2, 50).WithMessage(Messages.USERNAME_LENGTH)
                .Matches(@"^[a-zA-Z0-9]+$").WithMessage(Messages.USERNAME_LETTER)
                .Must(BeUniqueUsername).WithMessage(Messages.USERNAME_EXISTS);

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.PASSWORD_REQUIRED)
                .MinimumLength(6).WithMessage(Messages.PASSWORD_LENGTH);

            RuleFor(x => x.SchoolName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.NAME_REQUIRED)
                .Length(2, 50).WithMessage(Messages.NAME_LENGTH)
                .Matches(@"^[a-zA-Z\s]+$").WithMessage(Messages.NAME_LETTER);

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .EmailAddress().WithMessage(Messages.EMAIL_INVALID)
                .Must(BeUniqueEmail).WithMessage(Messages.EMAIL_EXISTS)
                .MaximumLength(40).WithMessage(StringHelper.FormatMaxLengthMessage(Messages.MAX, "Email", 40));

            RuleFor(x => x.Address)
                .MaximumLength(200).WithMessage(StringHelper.FormatMaxLengthMessage(Messages.MAX, "Địa chỉ", 200));

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^[0-9]+$")
                .WithMessage(StringHelper.FormatMessage(Messages.REGEX, "Số điện thoại"))
                .MaximumLength(15).WithMessage(StringHelper.FormatMaxLengthMessage(Messages.MAX, "Số điện thoại", 15));

            RuleFor(x => x.SubscriptionPlanId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Gói dịch vụ"))
                .Must(BeValidSubscriptionPlan).WithMessage(Messages.SUBSCRIPTION_PLAN_NOT_EXISTS);

            RuleFor(x => x.PaymentMethod)
                .NotEmpty()
                .When(x => x.SubscriptionPlanId != (int)Subscription.Trial && x.SubscriptionPlanId != null)
                .WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Phương thức thanh toán"));
        }

        private bool BeUniqueEmail(string email)
        {
            return !_context.Schools.Any(u => u.Email == email);
        }

        private bool BeUniqueUsername(string username)
        {
            return !_context.Schools.Any(u => u.Username == username);
        }

        private bool BeValidSubscriptionPlan(int? id)
        {
            return id.HasValue && _context.SubscriptionPlans.Any(s => s.Id == id.Value);
        }
    }
}