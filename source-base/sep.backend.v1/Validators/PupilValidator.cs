using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Extensions.EF;

namespace sep.backend.v1.Validators
{
    public class PupilValidator : AbstractValidator<CreatePupilDTO>
    {

        public PupilValidator()
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
            RuleFor(u => u.DonorName)
               .Cascade(CascadeMode.Stop)
               .NotEmpty().WithMessage(Responses.DonorNameRequired)
               .Length(2, 50).WithMessage(Messages.FIRST_NAME_LENGTH.Replace("{min}", "2").Replace("{max}", "50"))
               .Matches(@"^[\p{L}\s]+$").WithMessage(Messages.NAME_LETTER);
            RuleFor(u => u.DateOfBirth)
               .Cascade(CascadeMode.Stop)
               .NotEmpty().WithMessage(Messages.DATE_OF_BIRTH_REQUIRED)
               .Must(date => date <= DateTime.Now).WithMessage(Messages.DATE_OF_BIRTH_IN_THE_PAST);
            RuleFor(u => u.DonorPhoneNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.PHONE_NUMBER_REQUIRED)
                .Matches(Regex.PHONE).WithMessage(Messages.PHONE_NUMBER_INVALID); 
            RuleFor(u => u.Address)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.ADDRESS_REQUIRED);
        }

    }
    public class PupilUpdateValidator : AbstractValidator<UpdatePupilDTO>
    {
        private readonly ApplicationContext _context;

        public PupilUpdateValidator(ApplicationContext context)
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
            RuleFor(u => u.DonorName)
               .Cascade(CascadeMode.Stop)
               .NotEmpty().WithMessage(Responses.DonorNameRequired)
               .Length(2, 50).WithMessage(Messages.FIRST_NAME_LENGTH.Replace("{min}", "2").Replace("{max}", "50"))
               .Matches(@"^[\p{L}\s]+$").WithMessage(Messages.NAME_LETTER);
            RuleFor(u => u.DateOfBirth)
               .Cascade(CascadeMode.Stop)
               .NotEmpty().WithMessage(Messages.DATE_OF_BIRTH_REQUIRED)
               .Must(date => date <= DateTime.Now).WithMessage(Messages.DATE_OF_BIRTH_IN_THE_PAST);
            RuleFor(u => u.DonorPhoneNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.PHONE_NUMBER_REQUIRED)
                .Matches(Regex.PHONE).WithMessage(Messages.PHONE_NUMBER_INVALID);
            RuleFor(u => u.Address)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.ADDRESS_REQUIRED);

        }
    }

    public class PupilUpdateProfileValidator : AbstractValidator<UpdateProfilePupilDTO>
    {
        private readonly ApplicationContext _context;
        private readonly List<FileType> _allowedImageTypes = new List<FileType>
        {
            FileType.PNG, FileType.JPG, FileType.JPEG, FileType.GIF
        };
        private const long MaxFileSize = 5 * 1024 * 1024;
        public PupilUpdateProfileValidator(ApplicationContext context)
        {
            _context = context;

            RuleFor(u => u.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.NAME_REQUIRED)
                .Length(2, 50).WithMessage(Messages.NAME_LENGTH)
                .Matches(@"^[\p{L}\s]+$").WithMessage(Messages.NAME_LETTER);
            RuleFor(u => u.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.NAME_REQUIRED)
                .Length(2, 50).WithMessage(Messages.NAME_LENGTH)
                .Matches(@"^[\p{L}\s]+$").WithMessage(Messages.NAME_LETTER);
            RuleFor(u=>u.DonorName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.NAME_REQUIRED)
                .Length(2, 50).WithMessage(Messages.NAME_LENGTH)
                .Matches(@"^[\p{L}\s]+$").WithMessage(Messages.NAME_LETTER);
            RuleFor(u => u.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.EMAIL_REQUIRED)
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage(Messages.EMAIL_INVALID)
                .Must((dto, email) => BeUniqueEmail(email, dto.Id)).WithMessage(Messages.EMAIL_EXISTS);
            RuleFor(u => u.DonorPhoneNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.PHONE_NUMBER_REQUIRED)
                .Matches(@"^(03|05|07|08|09)\d{8}$").WithMessage(Messages.PHONE_NUMBER_INVALID);
            RuleFor(u => u.DateOfBirth)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.DATE_OF_BIRTH_REQUIRED)
                .Must(date => date <= DateTime.Now).WithMessage(Messages.DATE_OF_BIRTH_IN_THE_PAST);
            RuleFor(u => u.Address)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Messages.ADDRESS_REQUIRED);
            RuleFor(n => n.Image)
                .Cascade(CascadeMode.Stop)
            .Must(image => image == null || BeValidImageFile(image.FileName))
            .WithMessage("Các file hợp lệ PNG, JPG, JPEG, and GIF.")
            .Must(image => image == null || image.Length <= MaxFileSize)
            .WithMessage($"File vượt quá dung lượng {MaxFileSize / 1024 / 1024}MB.");
        }
        private bool BeValidImageFile(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower().TrimStart('.');

            if (Enum.TryParse(typeof(FileType), extension.ToUpper(), out var fileType))
            {
                return _allowedImageTypes.Contains((FileType)fileType);
            }

            return false;
        }

        private bool BeUniqueEmail(string email, int pupilId)
        {
            if (!_context.Pupils.Any(u => u.Id == pupilId))
            {
                throw new NotFoundException();
            }
            return !_context.Pupils.Any(u => u.Email == email && u.Id != pupilId);
        }
    }


}
