using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Validators
{
    public class BusStopValidator : AbstractValidator<CreateBusStopDTO>
    {
        public BusStopValidator()
        {
            RuleFor(busStop => busStop.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.REQUIRED.Replace("{attribute}", "Tên điểm dừng"))
                .MaximumLength(100).WithMessage(Messages.MAX.Replace("{attribute}", "Tên điểm dừng").Replace("{maxLength}", "100"));

            RuleFor(busStop => busStop.PickUpTime)
                 .Cascade(CascadeMode.Stop)
                 .NotNull().WithMessage(Messages.REQUIRED.Replace("{attribute}", "Thời gian đón"))
                 .GreaterThan(TimeSpan.Zero).WithMessage(Messages.INVALID.Replace("{attribute}", "Thời gian đón"));

            RuleFor(busStop => busStop.ReturnTime)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(Messages.REQUIRED.Replace("{attribute}", "Thời gian đưa về"))
                .GreaterThan(busStop => busStop.PickUpTime).WithMessage(Messages.INVALID_RETURNTIME);

            RuleFor(busStop => busStop.Address)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.REQUIRED.Replace("{attribute}", "Địa chỉ"))
                .MaximumLength(200).WithMessage(Messages.MAX.Replace("{attribute}", "Địa chỉ").Replace("{maxLength}", "200"));

            RuleFor(busStop => busStop.BusRouteId)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(Messages.REQUIRED.Replace("{attribute}", "Mã tuyến xe"))
                .GreaterThan(0).WithMessage(Messages.INVALID.Replace("{attribute}", "Mã tuyến xe"));

            RuleFor(busStop => busStop.Status)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(Messages.REQUIRED.Replace("{attribute}", "Trạng thái"));
        }
    }
}
