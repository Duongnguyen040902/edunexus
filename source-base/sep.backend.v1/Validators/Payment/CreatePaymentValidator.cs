using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.DTOs;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Helpers;

namespace sep.backend.v1.Validators.Payment;

public class CreatePaymentValidator : AbstractValidator<PaymentDTO>
{
    private readonly ApplicationContext _context;

    public CreatePaymentValidator(ApplicationContext context)
    {
        _context = context;

        RuleFor(x => x.InvoiceId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Hoá đơn"))
            .Must(BeExistingInvoiceId).WithMessage(StringHelper.FormatMessage(Messages.NOT_EXIST, "Hoá đơn"));

        RuleFor(x => x.Amount)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Số tiền"));

        RuleFor(x => x.PaymentDate)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Ngày thanh toán"));

        RuleFor(x => x.PaymentMethod)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Phương thức thanh toán"));

        RuleFor(x => x.Status)
            .Cascade(CascadeMode.Stop)
            .Must(BeValidStatusPayment)
            .WithMessage(StringHelper.FormatMessage(Messages.INVALID, "Trạng thái thanh toán"));
    }

    private bool BeExistingInvoiceId(int invoiceId)
    {
        return _context.Invoices.Any(i => i.Id == invoiceId);
    }

    private bool BeValidStatusPayment(int status)
    {
        return Enum.IsDefined(typeof(PaymentStatuses), status);
    }
}