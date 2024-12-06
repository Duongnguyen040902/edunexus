using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.DTOs;
using sep.backend.v1.Helpers;

namespace sep.backend.v1.Validators.SubscriptionPlan;

public class CreateSubscriptionPlanValidator : AbstractValidator<SubscriptionPlanDTO>
{
    public CreateSubscriptionPlanValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Tên gói"))
            .MaximumLength(255).WithMessage(StringHelper.FormatMaxLengthMessage(Messages.MAX, "Tên gói", 255));

        RuleFor(x => x.Price)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Giá"))
            .GreaterThan(0).WithMessage(StringHelper.FormatValueComparisonMessage(Messages.GREATER_THAN, "Giá", "0"));

        RuleFor(x => x.DurationDays)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Số ngày"))
            .GreaterThan(0).WithMessage(StringHelper.FormatValueComparisonMessage(Messages.GREATER_THAN, "Số ngày", "0"));
        
        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(255).WithMessage(StringHelper.FormatMaxLengthMessage(Messages.MAX, "Mô tả", 255));

        RuleFor(x => x.MaxActiveAccounts)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Số lượng tài khoản"))
            .GreaterThan(0).WithMessage(StringHelper.FormatValueComparisonMessage(Messages.GREATER_THAN, "Số ngày", "0")); 
    }
}