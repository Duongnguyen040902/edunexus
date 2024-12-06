using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.DTOs;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Helpers;
using System.Globalization;

namespace sep.backend.v1.Validators.Invoice;

public class CreateInvoiceValidator : AbstractValidator<CreateInvoiceDTO>
{
    private readonly ApplicationContext _context;

    public CreateInvoiceValidator(ApplicationContext context)
    {
        _context = context;

        RuleFor(x => x.IssueDate)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.DATE_REQUIRED, "Ngày phát hành"))
            .Must(BeValidDateFormat).WithMessage(Messages.DATE_INVALID)
            .LessThanOrEqualTo(x => x.DueDate)
            .WithMessage(
                StringHelper.FormatDateComparisonMessage(Messages.DATE_BEFORE, "Ngày phát hành", "ngày đến hạn"))
            .Must(BeInCurrentYear).WithMessage(Messages.DATE_NOT_IN_CURRENT_YEAR);

        RuleFor(x => x.DueDate)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.DATE_REQUIRED, "Ngày đến hạn"))
            .Must(BeValidDateFormat).WithMessage(Messages.DATE_INVALID)
            .GreaterThanOrEqualTo(x => x.IssueDate).WithMessage(
                StringHelper.FormatDateComparisonMessage(Messages.DATE_AFTER, "Ngày đến hạn", "ngày phát hành"))
            .Must(BeInCurrentYear).WithMessage(Messages.DATE_NOT_IN_CURRENT_YEAR);

        RuleFor(x => x.SubscriptionPlanId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0).WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Gói dịch vụ"))
            .Must(BeValidSchoolSubscriptionPlan)
            .WithMessage(StringHelper.FormatMessage(Messages.NOT_EXIST, "Gói dịch vụ"));

        RuleFor(x => x.SchoolId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0).WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Trường học"))
            .Must(BeValidSchool)
            .WithMessage(StringHelper.FormatMessage(Messages.NOT_EXIST, "Trường học"));

        RuleFor(x => x.Status)
            .Cascade(CascadeMode.Stop)
            .Must(BeValidStatusInvoice)
            .WithMessage(StringHelper.FormatMessage(Messages.INVALID, "Trạng thái gói dịch vụ"));

        RuleFor(x => x.PaymentMethod)
            .NotEmpty().When(x => x.Status == (int)InvoiceStatuses.Paid)
            .WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Phương thức thanh toán"));
    }

    private bool BeValidStatusInvoice(int status)
    {
        return Enum.IsDefined(typeof(InvoiceStatuses), status);
    }

    private bool BeValidSchoolSubscriptionPlan(int subscriptionPlanId)
    {
        return _context.SubscriptionPlans.Any(sp => sp.Id == subscriptionPlanId);
    }

    private bool BeValidSchool(int schoolId)
    {
        return _context.Schools.Any(s => s.Id == schoolId);
    }

    private bool BeInCurrentYear(DateTime date)
    {
        return date.Year == DateTime.Now.Year;
    }

    private bool BeValidDateFormat(DateTime date)
    {
        return DateTime.TryParseExact(
            date.ToString("yyyy-MM-dd"),
            "yyyy-MM-dd",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _
        );
    }
}