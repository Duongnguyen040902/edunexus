using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.DTOs;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Helpers;

namespace sep.backend.v1.Validators;

public class UpdateSchoolValidator : AbstractValidator<UpdateSchoolDTO>
{
    private readonly ApplicationContext _context;
    public UpdateSchoolValidator(ApplicationContext context)
    {
        _context = context;

        RuleFor(x => x.Address)
            .MaximumLength(100).WithMessage(StringHelper.FormatMaxLengthMessage(Messages.MAX, "Địa chỉ", 100));

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .EmailAddress().WithMessage(Messages.EMAIL_INVALID)
            .Must((dto, email) => BeUniqueEmail(dto.Id, email)).WithMessage(Messages.EMAIL_EXISTS)
            .MaximumLength(40).WithMessage(StringHelper.FormatMaxLengthMessage(Messages.MAX, "Email", 40));

        RuleFor(x => x.SchoolName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Messages.NAME_REQUIRED)
            .Length(2, 50).WithMessage(Messages.NAME_LENGTH)
            .Matches(@"^[a-zA-Z\s]+$").WithMessage(Messages.NAME_LETTER);
        RuleFor(x => x.PhoneNumber)
            .Cascade(CascadeMode.Stop)
            .Matches(@"^[0-9]+$")
            .WithMessage(StringHelper.FormatMessage(Messages.REGEX, "Số điện thoại"))
            .MaximumLength(15).WithMessage(StringHelper.FormatMaxLengthMessage(Messages.MAX, "Số điện thoại", 15));
        RuleFor(x => x.WebsiteLink)
            .Cascade(CascadeMode.Stop)
            .Matches(@"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=])?$")
            .WithMessage(StringHelper.FormatMessage(Messages.REGEX, "Website"))
            .MaximumLength(100).WithMessage(StringHelper.FormatMaxLengthMessage(Messages.MAX, "Website", 100));

        RuleFor(x => x.AccountStatus)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Trạng thái tài khoản"))
            .Must(BeValidStatusAccount).WithMessage(StringHelper.FormatMessage(Messages.INVALID, "Trạng thái tài khoản"));
        // RuleFor(x => x.SubscriptionPlanIds)
        //     .Cascade(CascadeMode.Stop)
        //     .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Gói dịch vụ"))
        //     .Must(x => x.TrueForAll(y => y > 0)).WithMessage(StringHelper.FormatMessage(Messages.INVALID, "Gói dịch vụ"))
        //     .Must(BeValidSubscriptionPlans).WithMessage(Messages.SUBSCRIPTION_PLAN_NOT_EXISTS); //TODO:QA - SubscriptionPlanIds
    }

    private bool BeValidStatusAccount(int status)
    {
        return Enum.IsDefined(typeof(StatusAccount), status);
    }
    private bool BeValidSubscriptionPlans(List<int> ids)
    {
        return ids.All(id => _context.SubscriptionPlans.Any(sp => sp.Id == id));
    }

    private bool BeUniqueEmail(int id, string email)
    {
        return !_context.Schools.Any(u => u.Email == email && u.Id != id);
    }
}