using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Extensions.EF;
using System.Threading.Tasks;

namespace sep.backend.v1.Requests.Policy
{
    public class ClassOfTeacherAndPupilRequirementHandler : AuthorizationHandler<ClassOfTeacherAndPupilRequirement>
    {
        private readonly ApplicationContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClassOfTeacherAndPupilRequirementHandler(ApplicationContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ClassOfTeacherAndPupilRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                context.Fail();
                return;
            }

            var query = httpContext.Request.Query;
            if (!query.TryGetValue("classId", out var classIdValue) || !int.TryParse(classIdValue.ToString(), out int classId))
            {
                context.Fail();
                return;
            }

            var userClaims = context.User.Claims;
            var teacherId = GetClaimValue(userClaims, "TeacherId");
            var pupilId = GetClaimValue(userClaims, "PupilId");
            var schoolId = GetClaimValue(userClaims, "SchoolId");

            if (schoolId == null)
            {
                context.Fail();
                return;
            }

            var activeSemester = await _context.Semesters.Include(x => x.SchoolYear)
                .FirstOrDefaultAsync(s => s.IsActive && s.SchoolYear.SchoolId == schoolId);
            if (activeSemester == null)
            {
                context.Fail();
                return;
            }

            if (teacherId != null && await IsClassEnrollmentExists(teacherId, classId, activeSemester.Id, true))
            {
                context.Succeed(requirement);
                return;
            }

            if (pupilId != null && await IsClassEnrollmentExists(pupilId, classId, activeSemester.Id, false))
            {
                context.Succeed(requirement);
                return;
            }

            context.Fail();
        }

        private int? GetClaimValue(IEnumerable<System.Security.Claims.Claim> claims, string claimType)
        {
            var claim = claims.FirstOrDefault(c => c.Type == claimType);
            return claim != null && int.TryParse(claim.Value, out int value) ? value : (int?)null;
        }

        private async Task<bool> IsClassEnrollmentExists(int? userId, int classId, int semesterId, bool isTeacher)
        {
            return await _context.ClassEnrollments.AnyAsync(ce =>
                (isTeacher ? ce.TeacherId == userId : ce.PupilId == userId) &&
                ce.ClassId == classId &&
                ce.SemesterId == semesterId);
        }
    }
}
