using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.DTOs;
using sep.backend.v1.Extensions.EF;

namespace sep.backend.v1.Validators.BusSupervisor
{
    public class BusSupervisorValidator: AbstractValidator<UpdateProfileBusSupervisorDTO>
    {
        private const long MaxFileSizeInBytes = 5 * 1024 *1024;
        public BusSupervisorValidator()
        {
            RuleFor(u => u.FirstName)
               .Cascade(CascadeMode.Stop)
               .NotEmpty().WithMessage(Responses.FirstNameRequired)
               .Length(2, 50).WithMessage(Messages.NAME_LENGTH)
               .Matches(@"^[\p{L}\s]+$").WithMessage(Messages.NAME_LETTER);
            RuleFor(u => u.LastName)
               .Cascade(CascadeMode.Stop)
               .NotEmpty().WithMessage(Responses.LastNameRequired)
               .Length(2, 50).WithMessage(Messages.NAME_LENGTH)
               .Matches(@"^[\p{L}\s]+$").WithMessage(Messages.NAME_LETTER);
            RuleFor(u => u.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.PHONE_NUMBER_REQUIRED)
                .Matches(Regex.PHONE).WithMessage(Messages.PHONE_NUMBER_INVALID);
            RuleFor(u => u.Address)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.ADDRESS_REQUIRED);
            RuleFor(u => u.Image)
               .Cascade(CascadeMode.Stop)
               .Must(IsValidFileType)
               .WithMessage("Tệp phải có định dạng PNG, JPG hoặc JPEG")
               .Must(file => file == null || file.Length <= MaxFileSizeInBytes)
               .WithMessage("Kích thước tập tin không được vượt quá 5 MB");
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
    public class BusSupervisorCreateValidator : AbstractValidator<CreateBusSupervisorDTO>
    {

        public BusSupervisorCreateValidator()
        {
            RuleFor(u => u.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Responses.FirstNameRequired)
                .Length(2, 15).WithMessage(Messages.FIRST_NAME_LENGTH.Replace("{min}", "2").Replace("{max}", "15"))
                .Matches(@"^[\p{L}]+$").WithMessage(Messages.FIRST_NAME_INVALID);
            RuleFor(u => u.LastName)
               .Cascade(CascadeMode.Stop)
               .NotEmpty().WithMessage(Responses.LastNameRequired)
               .Length(2, 25).WithMessage(Messages.LAST_NAME_LENGTH.Replace("{min}", "2").Replace("{max}", "25"))
               .Matches(@"^[\p{L}\s]+$").WithMessage(Messages.LAST_NAME_INVALID);
            RuleFor(u => u.Address)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.ADDRESS_REQUIRED);
            RuleFor(u => u.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.PHONE_NUMBER_REQUIRED)
                .Matches(Regex.PHONE).WithMessage(Messages.PHONE_NUMBER_INVALID);
        }
       
    }
    public class BusSupervisorUpdateValidator : AbstractValidator<UpdateBusSupervisorDTO>
    {
        private readonly ApplicationContext _context;
        public BusSupervisorUpdateValidator(ApplicationContext context)
        {
            _context = context;

            RuleFor(u => u.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Responses.FirstNameRequired)
                .Length(2, 15).WithMessage(Messages.FIRST_NAME_LENGTH.Replace("{min}", "2").Replace("{max}", "15"))
                .Matches(@"^[\p{L}]+$").WithMessage(Messages.FIRST_NAME_INVALID);
            RuleFor(u => u.LastName)
               .Cascade(CascadeMode.Stop)
               .NotEmpty().WithMessage(Responses.LastNameRequired)
               .Length(2, 25).WithMessage(Messages.LAST_NAME_LENGTH.Replace("{min}", "2").Replace("{max}", "25"))
               .Matches(@"^[\p{L}\s]+$").WithMessage(Messages.LAST_NAME_INVALID);
            RuleFor(u => u.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.PHONE_NUMBER_REQUIRED)
                .Matches(Regex.PHONE).WithMessage(Messages.PHONE_NUMBER_INVALID);
            RuleFor(u => u.Address)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.ADDRESS_REQUIRED);

        }
      
    }
}
