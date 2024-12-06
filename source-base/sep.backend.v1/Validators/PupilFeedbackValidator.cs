using FluentValidation;
using sep.backend.v1.DTOs;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Helpers;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Common.Const;

namespace sep.backend.v1.Validators
{
    public class PupilFeedbackValidator : AbstractValidator<PupilFeedbackDTO>
    {
        private readonly ApplicationContext _context;

        public PupilFeedbackValidator(ApplicationContext context)
        {
            _context = context;

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

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Nội dung"))
                .MaximumLength(300)
                .WithMessage(StringHelper.FormatMaxLengthMessage(Messages.MAX_LENGTH, "Nội dung", 300));

            RuleFor(x => x.Status)
                .Must(BeValidStatus)
                .WithMessage(StringHelper.FormatMessage(Messages.INVALID, "Trạng thái phản hồi"));

            RuleFor(x => x.Description)
                .Must((dto, description) => UniqueFeedbackPerPupilAndSemester(dto))
                .When(x => x.CreatedDate == null)
                .WithMessage(StringHelper.FormatMessage(Messages.IS_EXIST, "Phản hồi cho học sinh và học kỳ này"));

            RuleFor(x => x.Description)
                .Must((dto, description) => IsExistFeedback(dto))
                .When(x => x.CreatedDate != null)
                .WithMessage(StringHelper.FormatMessage(Messages.NOT_EXIST, "Phản hồi"));
        }

        private bool PupilExists(int pupilId)
        {
            return _context.Pupils.Any(x => x.Id == pupilId);
        }

        private bool SemesterExists(int semesterId)
        {
            return _context.Semesters.Any(x => x.Id == semesterId);
        }

        private bool UniqueFeedbackPerPupilAndSemester(PupilFeedbackDTO dto)
        {
            return !_context.PupilFeedbacks.Any(x => x.PupilId == dto.PupilId && x.SemesterId == dto.SemesterId && dto.CreatedDate == null);
        }

        private bool IsExistFeedback(PupilFeedbackDTO dto)
        {
            return _context.PupilFeedbacks.Any(x => x.PupilId == dto.PupilId && x.SemesterId == dto.SemesterId && dto.CreatedDate != null);
        }

        private bool BeValidStatus(int status)
        {
            return Enum.IsDefined(typeof(FeedbackStatus), status);
        }
    }

    public class RequestGetPupilFeedbackValidator : AbstractValidator<RequestGetPupilFeedbackDTO>
    {
        private readonly ApplicationContext _context;

        public RequestGetPupilFeedbackValidator(ApplicationContext context)
        {
            _context = context;

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
            RuleFor(x => x.PupilId)
                .Must((dto, pupilId) => IsExistFeedback(dto))
                .WithMessage(StringHelper.FormatMessage(Messages.NOT_EXIST, "Phản hồi"));
        }

        private bool PupilExists(int pupilId)
        {
            return _context.Pupils.Any(x => x.Id == pupilId);
        }

        private bool SemesterExists(int semesterId)
        {
            return _context.Semesters.Any(x => x.Id == semesterId);
        }

        private bool IsExistFeedback(RequestGetPupilFeedbackDTO dto)
        {
            return _context.PupilFeedbacks.Any(x => x.PupilId == dto.PupilId && x.SemesterId == dto.SemesterId);
        }
    }
}