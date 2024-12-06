using FluentValidation;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Common.Const;
using sep.backend.v1.DTOs;
using sep.backend.v1.Helpers;
using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.Validators
{

    public class NotificationValidator : AbstractValidator<AddNotificationDTO>
    {
        private readonly ApplicationContext _context;
        private readonly List<FileType> _allowedImageTypes = new List<FileType>
        {
            FileType.PNG, FileType.JPG, FileType.JPEG, FileType.GIF
        };
        private const long MaxFileSize = 5 * 1024 * 1024;
        private bool BeValidImageFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))

                return true;
            var extension = Path.GetExtension(fileName).ToLower().TrimStart('.');
            // Parse the extension to match with FileType enum
            if (Enum.TryParse(typeof(FileType), extension.ToUpper(), out var fileType))
            {
                return _allowedImageTypes.Contains((FileType)fileType);
            }

            return false;
        }

        public NotificationValidator(ApplicationContext context)
        {
            _context = context;
            RuleFor(n => n.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Tiêu đề"))
                .MaximumLength(100).WithMessage(StringHelper.FormatMaxLengthMessage(Messages.MAX, "Tiêu đề", 100));

            RuleFor(n => n.Descriptions)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Mô tả"))
                .MaximumLength(500).WithMessage(StringHelper.FormatMaxLengthMessage(Messages.MAX, "Mô tả", 500));

            RuleFor(n => n.CategoryId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Thể loại"))
                .Must(categoryId => BeUniqueId(categoryId ?? 0)).WithMessage("Thể loại không hợp lệ.");

            RuleForEach(n => n.FileImage)
                .ChildRules(notificationImage =>
                {
                    notificationImage.RuleFor(img => img.FileName)
                        .Must(BeValidImageFile)
                        .WithMessage("Không đúng định dạng. Các file hợp lệ PNG, JPG, JPEG, and GIF.");
                    notificationImage.RuleFor(img => img.Length)
                        .LessThanOrEqualTo(MaxFileSize)
                        .WithMessage($"File vượt quá dung lượng {MaxFileSize / 1024 / 1024}MB.");
                });
        }
        private bool BeUniqueId(int id)
        {
           
            
            return _context.NotificationCategories.Any(n => n.Id == id);
        }
    }

    public class UpdateNotificationValidate : AbstractValidator<UpdateNotificationDTO>
    {
        private readonly List<FileType> _allowedImageTypes = new List<FileType>
        {
            FileType.PNG, FileType.JPG, FileType.JPEG, FileType.GIF
        };
        private const long MaxFileSize = 5 * 1024 * 1024;
        private bool BeValidImageFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))

                return true;
            var extension = Path.GetExtension(fileName).ToLower().TrimStart('.');
            // Parse the extension to match with FileType enum
            if (Enum.TryParse(typeof(FileType), extension.ToUpper(), out var fileType))
            {
                return _allowedImageTypes.Contains((FileType)fileType);
            }

            return false;
        }
        public UpdateNotificationValidate()
        {
            RuleFor(n => n.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Tiêu đề"))
                .MaximumLength(100).WithMessage(StringHelper.FormatMaxLengthMessage(Messages.MAX, "Tiêu đề", 100));
            RuleFor(n => n.Descriptions)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Mô tả"))
                .MaximumLength(500).WithMessage(StringHelper.FormatMaxLengthMessage(Messages.MAX, "Tiêu đề", 500));
            RuleFor(n => n.CategoryId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Thể loại"));
            RuleForEach(n => n.FileImage)
                .ChildRules(notificationImage =>
                {
                    notificationImage.RuleFor(img => img.FileName)
                        .Must(BeValidImageFile)
                        .WithMessage("Không đúng định dạng. Các file hợp lệ PNG, JPG, JPEG, and GIF.");
                    notificationImage.RuleFor(img => img.Length)
                        .LessThanOrEqualTo(MaxFileSize)
                        .WithMessage($"File vượt quá dung lượng {MaxFileSize / 1024 / 1024}MB.");
                });
        }
    }
}