using FluentValidation;
using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Const;
using sep.backend.v1.DTOs;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Helpers;
using sep.backend.v1.Common.Enums;

namespace sep.backend.v1.Validators
{
    public class PupilScoreValidator : AbstractValidator<PupilScoreDTO>
    {
        private readonly ApplicationContext _context;

        public PupilScoreValidator(ApplicationContext context)
        {
            _context = context;

            RuleFor(x => x.SubjectId)
                .NotNull()
                .WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Môn học"))
                .Must(SubjectExists)
                .WithMessage(StringHelper.FormatMessage(Messages.NOT_EXIST, "Môn học"));

            RuleFor(x => x.PupilId)
                .NotNull()
                .WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Học sinh"))
                .Must(PupilExists)
                .WithMessage(StringHelper.FormatMessage(Messages.NOT_EXIST, "Học sinh"));

            RuleFor(x => x.SemesterId)
                .NotNull()
                .WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Học kỳ"))
                .Must(SemesterExists)
                .WithMessage(StringHelper.FormatMessage(Messages.NOT_EXIST, "Học kỳ"));

            RuleFor(x => x.Score)
                .NotNull()
                .WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Điểm số"))
                .InclusiveBetween(0, 10)
                .WithMessage(StringHelper.FormatMessage(Messages.INVALID, "Điểm số"));

            RuleFor(x => x.Status)
                .Must(BeValidStatus)
                .WithMessage(StringHelper.FormatMessage(Messages.INVALID, "Trạng thái điểm số"));

            RuleFor(x => x.Score)
                .Must((dto,score) => UniqueScorePerSubjectAndSemester(dto))
                .When(x => x.CreatedDate == null)
                .WithMessage(StringHelper.FormatMessage(Messages.IS_EXIST, "Điểm số cho môn học và học kỳ này"));

            RuleFor(x => x.Score)
                .Must((dto, score) => IsExistScore(dto))
                .When(x => x.CreatedDate != null)
                .WithMessage(StringHelper.FormatMessage(Messages.NOT_EXIST, "Điểm số"));
        }

        private bool SubjectExists(int subjectId)
        {
            return _context.Subjects.Any(x => x.Id == subjectId);
        }

        private bool PupilExists(int pupilId)
        {
            return _context.Pupils.Any(x => x.Id == pupilId);
        }

        private bool SemesterExists(int semesterId)
        {
            return _context.Semesters.Any(x => x.Id == semesterId);
        }

        private bool UniqueScorePerSubjectAndSemester(PupilScoreDTO dto)
        {
            return !_context.PupilScores.Any(x => x.SubjectId == dto.SubjectId && x.SemesterId == dto.SemesterId && x.PupilId == dto.PupilId && dto.CreatedDate == null);
        }

        private bool IsExistScore(PupilScoreDTO dto)
        {
            return _context.PupilScores.Any(x => x.SubjectId == dto.SubjectId && x.SemesterId == dto.SemesterId && x.PupilId == dto.PupilId && dto.CreatedDate != null);
        }

        private bool BeValidStatus(int status)
        {
            return Enum.IsDefined(typeof(ScoreStatus), status);
        }
    }
}