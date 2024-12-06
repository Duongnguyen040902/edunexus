using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Validators
{
    public class ClassValidator : AbstractValidator<AddClassDTO>
    {
        public ClassValidator()
        {
            RuleFor(c => c.Name)
               .Cascade(CascadeMode.Stop)
               .NotEmpty()
               .WithMessage(Messages.NAME_REQUIRED)
               .MaximumLength(5)
               .WithMessage(Messages.MAX.Replace("{attribute}", "Tên lớp").Replace("{maxLength}", "5"))
               .Matches("^[A-Z][a-zA-Z0-9]*$")
               .WithMessage(Messages.NAME_LETTER.Replace("{attribute}", "Tên lớp").Replace("{rule}", "phải bắt đầu bằng chữ cái in hoa"));
        }
    }

    public class UpdateClassValidator : AbstractValidator<UpdateClassDTO>
    {
        public UpdateClassValidator()
        {
            RuleFor(c => c.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(Messages.NAME_REQUIRED)
                .MaximumLength(5)
                .WithMessage(Messages.MAX.Replace("{attribute}", "Tên lớp").Replace("{maxLength}", "5"))
                .Matches("^[A-Z][a-zA-Z0-9]*$")
                .WithMessage(Messages.NAME_LETTER.Replace("{attribute}", "Tên lớp").Replace("{rule}", "phải bắt đầu bằng chữ cái in hoa"));
        }
    }

}