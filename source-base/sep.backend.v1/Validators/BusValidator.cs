using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Validators
{
    public class BusValidator : AbstractValidator<CreateBusDto>
    {
        public BusValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(Messages.REQUIRED.Replace("{attribute}", "Tên"))
                .Length(3, 100).WithMessage(Messages.NAME_LENGTH);

            RuleFor(x => x.DriverName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.REQUIRED.Replace("{attribute}", "Tên tài xế"))
                .MaximumLength(50).WithMessage(Messages.MAX_LENGTH);

            RuleFor(x => x.DriverPhone)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(Messages.REQUIRED.Replace("{attribute}", "Số điện thoại tài xế"))
                .Matches(@"^\+?[0-9]{9,11}$").WithMessage(Messages.PHONE_NUMBER_INVALID);

            RuleFor(x => x.LicensePlate)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(Messages.REQUIRED.Replace("{attribute}", "Biển số xe"))
                .MaximumLength(10).WithMessage(Messages.MAX_LENGTH)
                .Matches(@"^\d{2}[A-Z]{1,2}\d{1}-\d{4,5}$").WithMessage(Messages.LICENSE_PLATE_INVALID);

            RuleFor(x => x.SeatNumber)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(Messages.REQUIRED.Replace("{attribute}", "Số ghế"))
                .GreaterThan(4).WithMessage(Messages.SEAT_NUMBER_INVALID);

            RuleFor(x => x.Status)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(Messages.REQUIRED.Replace("{attribute}", "Trạng thái"));
        }
    }
}
