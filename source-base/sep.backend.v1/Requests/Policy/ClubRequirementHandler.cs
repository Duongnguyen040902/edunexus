using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Extensions.EF;
using System.Threading.Tasks;

namespace sep.backend.v1.Requests.Policy
{
    public class ClubRequirementHandler : AuthorizationHandler<ClubRequirement>
    {
        private readonly ApplicationContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClubRequirementHandler(ApplicationContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ClubRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                context.Fail();
                return;
            }

            var query = httpContext.Request.Query;
            if (!query.TryGetValue("clubId", out var clubIdValue) || !int.TryParse(clubIdValue.ToString(), out int clubId))
            {
                context.Fail();
                return;
            }

            var teacherIdClaim = context.User.FindFirst(c => c.Type == "TeacherId");
            var schoolIdClaim = context.User.FindFirst(c => c.Type == "SchoolId");
            if (teacherIdClaim == null || !int.TryParse(teacherIdClaim.Value, out int teacherId) ||
                schoolIdClaim == null || !int.TryParse(schoolIdClaim.Value, out int schoolId))
            {
                context.Fail();
                return;
            }

            var activeSemester = await _context.Semesters.Include(x => x.SchoolYear).FirstOrDefaultAsync(s => s.IsActive && s.SchoolYear.SchoolId == schoolId);
            if (activeSemester == null)
            {
                context.Fail();
                return;
            }

            var clubEnrollmentExists = await _context.ClubEnrollments.AnyAsync(ce => ce.TeacherId == teacherId && ce.ClubId == clubId && ce.SemesterId == activeSemester.Id);
            if (clubEnrollmentExists)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}