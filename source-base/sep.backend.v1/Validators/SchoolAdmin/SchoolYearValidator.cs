using FluentValidation;
using sep.backend.v1.DTOs;
using sep.backend.v1.Data.Entities;
using System;
using sep.backend.v1.Services.UnitOfWork;
using System.Linq;
using sep.backend.v1.Services.UnitOfWorks;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Helpers;

namespace sep.backend.v1.Validators.SchoolAdmin
{
    public class SchoolYearValidator : AbstractValidator<CreateAndUpdateSchoolYearDTO>
    {
        private readonly ApplicationContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SchoolYearValidator(ApplicationContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            RuleFor(x => x.StartDate)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Ngày bắt đầu"))
                .Must((dto, startDate) => StartDateAfterEndDateSchoolYearBefore(dto, startDate))
                .WithMessage(dto =>
                {
                    DateTime? previousEndDate = GetPreviousSchoolYearEndDate(dto.Id);
                    return $"Ngày bắt đầu phải sau ngày kết thúc của năm học trước ({previousEndDate?.ToString("dd/MM/yyyy")}).";
                })
                .Must((dto, startDate) =>
                {
                    var (isValid, minStartDate) = ValidateSemesterStartDate(dto, startDate);
                    if (!isValid)
                    {
                        _httpContextAccessor.HttpContext.Items["MinStartDate"] = minStartDate; // Use IHttpContextAccessor to store the value
                    }
                    return isValid;
                })
                .WithMessage(dto =>
                {
                    var minStartDate = _httpContextAccessor.HttpContext.Items["MinStartDate"] as DateTime?;
                    return $"Ngày bắt đầu của năm học phải nằm ngoài khoảng thời gian của các học kỳ: {minStartDate?.ToString("MM/dd/yyyy")}.";
                });
            RuleFor(x => x.EndDate)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Ngày kết thúc"))
                .Must(endDate => endDate >= DateTime.Today).WithMessage("Ngày kết thúc phải sau ngày hôm nay")
                .Must((dto, endDate) => EndDateAfterStartDate(dto, endDate)).WithMessage("Ngày kết thúc phải sau ngày bắt đầu tối thiểu 180 ngày.")
                .Must((dto, endDate) =>
                {
                    var (isValid, maxEndDate) = ValidateSemesterEndDate(dto, endDate);
                    if (!isValid)
                    {
                        _httpContextAccessor.HttpContext.Items["MaxEndDate"] = _context.Semesters
                            .Where(s => s.SchoolYearId == dto.Id)
                            .Max(s => s.EndDate);
                    }
                    return isValid;
                })
                .WithMessage(dto =>
                {
                    var maxEndDate = _httpContextAccessor.HttpContext.Items["MaxEndDate"] as DateTime?;
                    return $"Ngày kết thúc của năm học phải nằm ngoài khoảng thời gian của các học kỳ: {maxEndDate?.ToString("MM/dd/yyyy")}.";
                });
                }

        private bool EndDateAfterStartDate(CreateAndUpdateSchoolYearDTO dto, DateTime? endDate)
        {
            return endDate.HasValue && (endDate.Value - dto.StartDate).TotalDays >= 180;
        }

        protected int? SchoolId
        {
            get
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null && httpContext.Items["SchoolId"] != null && int.TryParse(httpContext.Items["SchoolId"].ToString(), out int schoolId))
                {
                    return schoolId;
                }
                return null;
            }
        }

        private bool StartDateAfterEndDateSchoolYearBefore(CreateAndUpdateSchoolYearDTO dto, DateTime? startDate)
        {
            DateTime? previousEndDate = GetPreviousSchoolYearEndDate(dto.Id);
            return startDate.HasValue && previousEndDate.HasValue && startDate.Value > previousEndDate.Value;
        }

        private DateTime? GetPreviousSchoolYearEndDate(int? currentSchoolYearId)
        {
            var query = _context.SchoolYears
                .Where(sy => sy.SchoolId == SchoolId && sy.EndDate != default && sy.Id != currentSchoolYearId);

            if (currentSchoolYearId != 0)
            {
                query = query.Where(sy => sy.Id < currentSchoolYearId);
            }

            return query
                .OrderByDescending(sy => sy.EndDate)
                .Select(sy => sy.EndDate)
                .FirstOrDefault();
        }

        private (bool isValid, DateTime? minStartDate) ValidateSemesterStartDate(CreateAndUpdateSchoolYearDTO dto, DateTime? startDate)
        {
            var semesters = _context.Semesters.Where(s => s.SchoolYearId == dto.Id).ToList();
            if (!semesters.Any())
            {
                return (true, null);
            }

            var minStartDate = semesters.Min(s => s.StartDate);

            return (startDate <= minStartDate, minStartDate);
        }

        private (bool isValid, DateTime? maxEndDate) ValidateSemesterEndDate(CreateAndUpdateSchoolYearDTO dto, DateTime? endDate)
        {
            var semesters = _context.Semesters.Where(s => s.SchoolYearId == dto.Id).ToList();
            if (!semesters.Any())
            {
                return (true, null);
            }
            var maxEndDate = semesters.Max(s => s.EndDate);

            return (endDate >= maxEndDate, maxEndDate);
        }

    }
}
