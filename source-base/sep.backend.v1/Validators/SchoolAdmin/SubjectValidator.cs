using FluentValidation;
using sep.backend.v1.Common.Const;
using sep.backend.v1.DTOs;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Helpers;

namespace sep.backend.v1.Validators.SchoolAdmin;

public class SubjectValidator : AbstractValidator<SubjectDTO>
{
    private readonly ApplicationContext _context;

    public SubjectValidator(ApplicationContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Tên môn học"))
            .MaximumLength(50).WithMessage(StringHelper.FormatMaxLengthMessage(Messages.MAX, "Tên môn học", 50))
            .Must((subject, name) => BeUniqueName(subject.Id, name, subject.SchoolId))
            .WithMessage(StringHelper.FormatMessage(Messages.UNIQUE, "Tên môn học"));

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage(StringHelper.FormatMessage(Messages.REQUIRED, "Tên viết tắt"))
            .MaximumLength(50).WithMessage(StringHelper.FormatMaxLengthMessage(Messages.MAX, "Tên viết tắt", 50));
    }

    private bool BeUniqueName(int id, string name, int schoolId)
    {
        return !_context.Subjects.Any(s => s.Name == name && s.SchoolId == schoolId && s.Id != id);
    }
}