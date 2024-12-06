using FluentValidation;
using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Helpers;
using sep.backend.v1.Requests.Auth;

namespace sep.backend.v1.Validators;

public class VerifyFirstLoginValidator : AbstractValidator<VerifyFirstLoginRequest>
{
    private readonly ApplicationContext _context;

    public VerifyFirstLoginValidator(ApplicationContext context)
    {
        _context = context;

        RuleFor(x => x.Mode)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(StringHelper.FormatMessage(Messages.INVALID, "Mode"));

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(Messages.EMAIL_REQUIRED)
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage(Messages.EMAIL_INVALID)
            .Must((request, email) => !EmailExists(request.Mode, email, request.id))
            .WithMessage(StringHelper.FormatMessage(Messages.EMAIL_EXISTS, "Email"));
    }

    private bool EmailExists(int mode, string email, int userId)
    {
        var modeFind = GetByMode(mode);

        var userRepositoryMapping = new Dictionary<ModeLogin, Func<string, int, object?>>
        {
            {
                ModeLogin.SuperAdmin,
                (email, userId) => _context.SuperAdmins.FirstOrDefault(x => x.Email == email && x.Id != userId)
            },
            {
                ModeLogin.SchoolAdmin,
                (email, userId) => _context.Schools.FirstOrDefault(x => x.Email == email && x.Id != userId)
            },
            {
                ModeLogin.Donnor,
                (email, userId) => _context.Pupils.FirstOrDefault(x => x.Email == email && x.Id != userId)
            },
            {
                ModeLogin.Teacher,
                (email, userId) => _context.Teachers.FirstOrDefault(x => x.Email == email && x.Id != userId)
            },
            {
                ModeLogin.BusSuperVisor,
                (email, userId) => _context.BusSupervisors.FirstOrDefault(x => x.Email == email && x.Id != userId)
            }
        };

        if (userRepositoryMapping.TryGetValue(modeFind, out var findUserFunc))
        {
            var user = findUserFunc(email, userId);
            return user != null;
        }

        throw new ArgumentOutOfRangeException(nameof(modeFind), "Invalid mode value");
    }

    private ModeLogin GetByMode(int mode)
    {
        return mode switch
        {
            0 => ModeLogin.SuperAdmin,
            1 => ModeLogin.SchoolAdmin,
            2 => ModeLogin.Donnor,
            3 => ModeLogin.Teacher,
            4 => ModeLogin.BusSuperVisor,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), "Invalid mode value")
        };
    }
}