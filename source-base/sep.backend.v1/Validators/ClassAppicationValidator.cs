using FluentValidation;
using sep.backend.v1.DTOs;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Helpers;
using sep.backend.v1.Common.Enums;

namespace sep.backend.v1.Validators.Teacher
{
    public class ClassAppicationValidator : AbstractValidator<CreateAndUpdateClassApplicationDTO>
    {
        public ClassAppicationValidator()
        {
            RuleFor(x => x.SemesterId)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Kỳ học"));

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Tiêu đề"))
                .MaximumLength(100).WithMessage(StringHelper.FormatMaxLengthMessage(Messages.MAX, "Tiêu đề", 50));

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Nội dung"))
                .MaximumLength(500).WithMessage(StringHelper.FormatMaxLengthMessage(Messages.MAX, "Mô tả", 500));

            RuleFor(x => x.ApplicationCategoryId)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Danh mục đơn"));
        }
    }

    public class ResponeClassApplicationValidator : AbstractValidator<ResponeClassApplicationDTO>
    {
        public ResponeClassApplicationValidator()
        {
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Mã Đơn"));

            RuleFor(x => x.Response)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Phản hồi"))
                .MaximumLength(300).WithMessage(StringHelper.FormatMaxLengthMessage(Messages.MAX, "Phản hồi", 300));

            RuleFor(x => x.Status)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Trạng thái"))
                .Must(BeValidStatusAccount).WithMessage(StringHelper.FormatMessage(Messages.INVALID, "Trạng thái"));
        }
        private bool BeValidStatusAccount(int status)
        {
            return Enum.IsDefined(typeof(ResponseApplicationStatus), status);
        }
    }
}
