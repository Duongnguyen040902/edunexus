using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.DTOs;
using sep.backend.v1.Helpers;
using sep.backend.v1.Data.Entities;
using System;
using System.Linq;
using sep.backend.v1.Extensions.EF;

namespace sep.backend.v1.Validators.SchoolAdmin
{
    public class SemesterValidator : AbstractValidator<CreateAndUpdateSemesterDTO>
    {
        private readonly ApplicationContext _context;

        public SemesterValidator(ApplicationContext context)
        {
            _context = context;

            RuleFor(x => x.SemesterName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Tên kì học"))
                .MaximumLength(50).WithMessage("Tên kì học không được vượt quá 50 ký tự");

            RuleFor(x => x.SemesterCode)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Code kì học"))
                .MaximumLength(50).WithMessage("Mã kì học không được vượt quá 50 ký tự");


            RuleFor(x => x.StartDate)
                .Cascade(CascadeMode.Stop).Must(startDate => startDate.ToString()!="")
    .WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Ngày bắt đầu"))
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Ngày bắt đầu"))
                
                .Must((dto, startDate) => BeWithinSchoolYearDates(dto, startDate, true))
                .WithMessage(dto =>
                {
                    var schoolYearDates = GetSchoolYearDates(dto.SchoolYearId);
                    return $"Ngày bắt đầu phải nằm trong khoảng thời gian của năm học ({schoolYearDates.Value.startDate:dd/MM/yyyy} - {schoolYearDates.Value.endDate:dd/MM/yyyy}).";
                        
                })
                .Must((dto, startDate) => StartDateAfterPreviousSemesterEndDate(dto, startDate))
                .WithMessage(dto =>
                {
                    DateTime? previousEndDate = GetPreviousSemesterEndDate(dto);
                    return $"Ngày bắt đầu của kì học mới phải sau ngày kết thúc của kì học trước đó ({previousEndDate.Value:dd/MM/yyyy}).";
                });

            RuleFor(x => x.EndDate)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Ngày kết thúc"))
                .Must(endDate => endDate>= DateTime.Today).WithMessage("Ngày kết thúc phải sau ngày hôm nay")
                .Must((dto, endDate) => BeWithinSchoolYearDates(dto, endDate, false))
                .WithMessage(dto =>
                {
                    var schoolYearDates = GetSchoolYearDates(dto.SchoolYearId);
                    return $"Ngày kết thúc phải nằm trong khoảng thời gian của năm học ({schoolYearDates.Value.startDate:dd/MM/yyyy} - {schoolYearDates.Value.endDate:dd/MM/yyyy}).";
                })
                .Must((dto, endDate) => EndDateAfterStartDate(dto, endDate))
                .WithMessage("Ngày kết thúc phải sau ngày bắt đầu ít nhất 30 ngày.");
        }

        private bool BeWithinSchoolYearDates(CreateAndUpdateSemesterDTO dto, DateTime? date, bool isStartDate)
        {
            if (!dto.SchoolYearId.HasValue || !date.HasValue)
            {
                return true;
            }

            var schoolYear = _context.SchoolYears.Find(dto.SchoolYearId.Value);
            if (schoolYear == null)
            {
                return false;
            }

            return isStartDate
                ? date >= schoolYear.StartDate && date <= schoolYear.EndDate
                : date <= schoolYear.EndDate && date >= schoolYear.StartDate;
        }

        private bool EndDateAfterStartDate(CreateAndUpdateSemesterDTO dto, DateTime? endDate)
        {
            return endDate.HasValue && dto.StartDate.HasValue && (endDate.Value - dto.StartDate.Value).TotalDays >= 30;
        }

        private bool StartDateAfterPreviousSemesterEndDate(CreateAndUpdateSemesterDTO dto, DateTime? startDate)
        {
            var previousSemesterEndDate = GetPreviousSemesterEndDate(dto);
            return previousSemesterEndDate == null || startDate > previousSemesterEndDate;
        }

        private (DateTime startDate, DateTime endDate)? GetSchoolYearDates(int? schoolYearId)
        {
            var schoolYear = _context.SchoolYears.Find(schoolYearId);
            return (schoolYear.StartDate, schoolYear.EndDate);
        }

        private DateTime? GetPreviousSemesterEndDate(CreateAndUpdateSemesterDTO dto)
        {
            var query = _context.Semesters
            .Where(s => s.SchoolYearId == dto.SchoolYearId && s.Id != dto.Id);

            if (dto.Id != 0)
            {
                query = query.Where(s => s.Id < dto.Id);
            }

            return query
                .OrderByDescending(s => s.EndDate)
                .Select(s => s.EndDate)
                .FirstOrDefault();
        }
    }
}
