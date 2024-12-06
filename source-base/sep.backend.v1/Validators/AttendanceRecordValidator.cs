using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.DTOs;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Helpers;

namespace sep.backend.v1.Validators
{
    public class AttendanceRecordValidator : AbstractValidator<AttendanceRecordDTO>
    {
        private readonly ApplicationContext _context;
        public AttendanceRecordValidator(ApplicationContext context)
        {
            _context = context;
            RuleFor(x => x.Id)
                .Must(IsEsixtAttendanceRecord).WithMessage(StringHelper.FormatMessage(Messages.NOT_EXIST, "Hồ sơ điểm danh"));
            RuleFor(x => x.PupilId)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Học sinh"))
                .Must(BeValidPupil).WithMessage(StringHelper.FormatMessage(Messages.NOT_EXIST, "Học sinh"));

            RuleFor(x => x.ClassId)
                .Must(BeValidClass).WithMessage(StringHelper.FormatMessage(Messages.NOT_EXIST, "Lớp"));

            RuleFor(x => x.ClubId)
                .Must(BeValidClub).WithMessage(StringHelper.FormatMessage(Messages.NOT_EXIST, "Câu lạc bộ"));

            RuleFor(x => x.BusId)
                .Must(BeValidBus).WithMessage(StringHelper.FormatMessage(Messages.NOT_EXIST, "Xe bus"));

            RuleFor(x => x.IsAttend)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Trạng thái điểm danh"));

            RuleFor(x => x.AttendanceSession)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Ca điểm danh"))
                .Must(BeValidAttendanceType).WithMessage(StringHelper.FormatMessage(Messages.INVALID, "Ca điểm danh")); ;

            RuleFor(x => x.AttendanceType)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Loại điểm danh"))
                .Must(BeValidAttendanceType).WithMessage(StringHelper.FormatMessage(Messages.INVALID, "Loại điểm danh"));

            RuleFor(x => x.Feedback)
                .MaximumLength(100).WithMessage(StringHelper.FormatMaxLengthMessage(Messages.MAX, "Ghi chú", 100));

            RuleFor(x => x.CreatedDate)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Ngày tạo"))
                .Must((dto, createDate) => IsExsitAttendanceDate(dto)).WithMessage(StringHelper.FormatMessage(Messages.IS_EXIST, "Hồ sơ hôm nay"));
        }

        private bool BeValidPupil(int pupilId)
        {
            return _context.Pupils.Any(p => p.Id == pupilId);
        }

        private bool BeValidClass(int? classId)
        {
            return classId == null || _context.Classes.Any(c => c.Id == classId);
        }

        private bool BeValidClub(int? clubId)
        {
            return clubId == null || _context.Clubs.Any(c => c.Id == clubId);
        }

        private bool BeValidBus(int? busId)
        {
            return busId == null || _context.Buses.Any(b => b.Id == busId);
        }

        private bool IsExsitAttendanceDate(AttendanceRecordDTO dto)
        {
            return !_context.AttendanceRecords.Any(ar =>
                (ar.ClassId == null || ar.ClassId == dto.ClassId) &&
                (ar.ClubId == null || ar.ClubId == dto.ClubId) &&
                (ar.BusId == null || ar.BusId == dto.BusId) &&
                ar.CreatedDate.Date == dto.CreatedDate.Date &&
                ar.AttendanceSession == dto.AttendanceSession &&
                ar.AttendanceType == dto.AttendanceType &&
                dto.Id == 0);
        }

        private bool IsEsixtAttendanceRecord(int? id)
        {
            if (id == 0)
            {
                return true;
            }
            return _context.AttendanceRecords.Any(ar => ar.Id == id);
        }

        private bool BeValidAttendanceType(int attendanceType)
        {
            return Enum.IsDefined(typeof(AttendanceType), attendanceType);
        }
    }
}
