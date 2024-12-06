using FluentValidation;
using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Const;
using sep.backend.v1.DTOs;
using sep.backend.v1.Data;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Helpers;

namespace sep.backend.v1.Validators
{
    public class TimeTableValidator : AbstractValidator<TimeTableDTO>
    {
        private readonly ApplicationContext _context;

        public TimeTableValidator(ApplicationContext context)
        {
            _context = context;

            RuleFor(t => t.ClassId)
                .NotEmpty()
                .WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Lớp học"))
                .Must(ClassExists).WithMessage(StringHelper.FormatMessage(Messages.NOT_EXIST, "Lớp học"));

            RuleFor(t => t.SemesterId)
                .NotEmpty()
                .WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Học kỳ"))
                .Must(SemesterExists)
                .WithMessage(StringHelper.FormatMessage(Messages.NOT_EXIST, "Học kỳ"));

            RuleFor(t => t.TimeSlotId)
                .NotEmpty()
                .WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Tiết"))
                .Must(TimeSlotExists)
                .WithMessage(StringHelper.FormatMessage(Messages.NOT_EXIST, "Tiết"));

            RuleFor(t => t.SubjectId)
                .NotEmpty()
                .WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Môn học"))
                .Must(SubjectExists)
                .WithMessage(StringHelper.FormatMessage(Messages.NOT_EXIST, "Môn học"));

            RuleFor(t => t.DayOfWeek)
                .NotEmpty()
                .WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Ngày trong tuần"))
                .Must(BeValidDayOfWeek)
                .WithMessage(StringHelper.FormatMessage(Messages.INVALID, "Ngày trong tuần"));
            ;
        }

        private bool ClassExists(int classId)
        {
            return _context.Classes.Any(x => x.Id == classId);
        }

        private bool SemesterExists(int semesterId)
        {
            return _context.Semesters.Any(x => x.Id == semesterId);
        }

        private bool TimeSlotExists(int timeSlotId)
        {
            return _context.TimeSlots.Any(x => x.Id == timeSlotId);
        }

        private bool SubjectExists(int subjectId)
        {
            return _context.Subjects.Any(x => x.Id == subjectId);
        }

        private bool BeValidDayOfWeek(int dayOfWeek)
        {
            return Enum.IsDefined(typeof(DayOfWeek), dayOfWeek);
        }
    }
}

