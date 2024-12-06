using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Validators
{
    public class ClubValidator : AbstractValidator<CreateClubDTO>
    {
        public ClubValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.REQUIRED.Replace("{attribute}", "Tên câu lạc bộ"))
                .MaximumLength(100).WithMessage(Messages.MAX.Replace("{attribute}", "Tên câu lạc bộ").Replace("{maxLength}", "100"));

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.REQUIRED.Replace("{attribute}", "Mô tả"))
                .Must(HaveMaxWords).WithMessage(Messages.MAX.Replace("{attribute}", "Mô tả").Replace("{maxLength}", "200 từ"));

            RuleFor(x => x.Status)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(Messages.REQUIRED.Replace("{attribute}", "Trạng thái"))
                .InclusiveBetween(0, 1).WithMessage(Messages.INVALID.Replace("{attribute}", "Trạng thái"));
        }
        private bool HaveMaxWords(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return true;

            var wordCount = description.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
            return wordCount <= 200;
        }
    }
}
