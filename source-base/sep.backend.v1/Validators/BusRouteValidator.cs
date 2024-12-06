using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Validators
{
    public class BusRouteValidator : AbstractValidator<CreateBusRouteDto>
    {
        public BusRouteValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.NAME_REQUIRED)
                .Length(3, 100).WithMessage(Messages.NAME_LENGTH);

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.REQUIRED.Replace("{attribute}", "Mô tả"))
                .MaximumLength(200).WithMessage(Messages.MAX.Replace("{attribute}", "Mô tả").Replace("{maxLength}", "200"));

            RuleFor(x => x.Status)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(Messages.REQUIRED.Replace("{attribute}", "Trạng thái"))
                .InclusiveBetween(1, 3).WithMessage(Messages.INVALID.Replace("{attribute}", "Trạng thái"));
        }
    }
}

