using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.DTOs;
using sep.backend.v1.Requests.Excel;
using System.IO;

namespace sep.backend.v1.Validators
{
    public class UploadExcelValidator : AbstractValidator<UploadExcelRequest>
    {
        private readonly List<string> _allowedFileExtensions = new List<string>
        {
            ".xlsx",
            ".xls"
        };

        private const long MaxFileSize = 30 * 1024; 

        public UploadExcelValidator()
        {
            RuleFor(u => u.File)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(Responses.FileRequired)
                .Must(BeAValidFile).WithMessage("Tệp phải có định dạng XLSX hoặc XLS") 
                .Must(HaveValidSize).WithMessage(Messages.FILE_SIZE_EXCEEDED.Replace("{maxSize}", MaxFileSize.ToString())); 
        }

        private bool BeAValidFile(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            return _allowedFileExtensions.Contains(extension);
        }

        private bool HaveValidSize(IFormFile file)
        {
            return file.Length <= MaxFileSize;
        }
    }
}
