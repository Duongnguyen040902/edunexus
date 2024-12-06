using FluentValidation;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.DTOs;
using sep.backend.v1.Common.Const;

namespace sep.backend.v1.Validators
{
    public class SchoolValidator : AbstractValidator<UpdateInfoSchoolDTO>
    {
        private const long MaxFileSizeInBytes = 2 * 1024 * 1024;

        public SchoolValidator()
        {
            RuleFor(s => s.Name)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(Messages.NAME_REQUIRED)
                .NotEmpty().WithMessage(Messages.NAME_REQUIRED)
                .MinimumLength(6).WithMessage(Messages.MIN.Replace("{attribute}", "Tên").Replace("{minLength}", "6"))
                .MaximumLength(100).WithMessage(Messages.MAX.Replace("{attribute}", "Tên").Replace("{maxLength}", "100"));

            RuleFor(s => s.Address)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(Messages.ADDRESS_REQUIRED)
                .NotEmpty().WithMessage(Messages.ADDRESS_REQUIRED)
                .MaximumLength(200).WithMessage(Messages.MAX.Replace("{attribute}", "Địa chỉ").Replace("{maxLength}", "200"));

            RuleFor(s => s.PhoneNumber)
                 .Cascade(CascadeMode.Stop)
                 .NotNull().WithMessage(Messages.PHONE_NUMBER_REQUIRED)
                 .NotEmpty().WithMessage(Messages.PHONE_NUMBER_REQUIRED)
                 .Matches(@"^(\+84|0)\d{9}$").WithMessage(Messages.PHONE_NUMBER_INVALID)
                 .Length(10, 13).WithMessage(Messages.PHONE_NUMBER_LENGTH.Replace("{min}", "10").Replace("{max}", "13"));

            RuleFor(s => s.PrincipalName)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(Messages.NAME_REQUIRED)
                .NotEmpty().WithMessage(Messages.NAME_REQUIRED)
                .MaximumLength(100).WithMessage(Messages.MAX.Replace("{attribute}", "Tên hiệu trưởng").Replace("{maxLength}", "100"));

            RuleFor(s => s.PrincipalPhone)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(Messages.PHONE_NUMBER_REQUIRED)
                .NotEmpty().WithMessage(Messages.PHONE_NUMBER_REQUIRED)
                .Matches(@"^(\+84|0)\d{9}$").WithMessage(Messages.PHONE_NUMBER_INVALID)
                .Length(10, 13).WithMessage(Messages.PHONE_NUMBER_LENGTH.Replace("{min}", "10").Replace("{max}", "13"));

            RuleFor(s => s.WebsiteLink)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(200).WithMessage(Messages.MAX.Replace("{attribute}", "Liên kết trang web").Replace("{maxLength}", "200"))
                .Matches(@"^https?:\/\/.*").WithMessage(Messages.INVALID.Replace("{attribute}", "Liên kết trang web"));

            RuleFor(s => s.ImageFile)
                 .Cascade(CascadeMode.Stop)
                .Must(IsValidFileType).WithMessage("Tệp phải có định dạng PNG, JPG hoặc JPEG")
                .Must(file => file == null || file.Length <= MaxFileSizeInBytes)
                .WithMessage(Messages.FILE_SIZE_EXCEEDED.Replace("{maxSize}", "2"));

            RuleFor(s => s.StandardCode)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(Messages.REQUIRED.Replace("{attribute}", "Mã chuẩn"))
                .MaximumLength(50).WithMessage(Messages.MAX.Replace("{attribute}", "Mã chuẩn").Replace("{maxLength}", "50"));

            RuleFor(s => s.DateOfEstablishment)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(Messages.DATE_OF_BIRTH_REQUIRED)
                .LessThanOrEqualTo(DateTime.Now).WithMessage(Messages.DATE_OF_BIRTH_IN_THE_PAST);

            RuleFor(s => s.FAX)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(Messages.REQUIRED.Replace("{attribute}", "FAX"))
                .Matches(@"^\+?[0-9]*$").WithMessage(Messages.INVALID.Replace("{attribute}", "FAX"));
        }

        private bool IsValidFileType(IFormFile file)
        {
            if (file == null) return true;

            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            return fileExtension switch
            {
                ".png" => Enum.IsDefined(typeof(FileType), FileType.PNG),
                ".jpg" => Enum.IsDefined(typeof(FileType), FileType.JPG),
                ".jpeg" => Enum.IsDefined(typeof(FileType), FileType.JPEG),
                _ => false
            };
        }
    }
}
