using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Extensions.EF;
using System.Linq;
using System.Threading.Tasks;

namespace sep.backend.v1.Requests.Policy
{
    public class MultiRoleRequirementHandler : AuthorizationHandler<MultiRoleRequirement>
    {
        private readonly ApplicationContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MultiRoleRequirementHandler(ApplicationContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MultiRoleRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                context.Fail();
                return;
            }

            var query = httpContext.Request.Query;
            if (!query.TryGetValue("entityId", out var entityIdValue) || !int.TryParse(entityIdValue.ToString(), out int entityId) ||
                !query.TryGetValue("session", out var typeValue) || !int.TryParse(typeValue.ToString(), out int type))
            {
                context.Fail();
                return;
            }

            var schoolIdClaim = context.User.FindFirst(c => c.Type == "SchoolId");
            if (schoolIdClaim == null || !int.TryParse(schoolIdClaim.Value, out int schoolId))
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

            bool hasAccess = false;

            var supervisorIdClaim = context.User.FindFirst(c => c.Type == "BusSupervisorId");
            if (supervisorIdClaim != null && int.TryParse(supervisorIdClaim.Value, out int supervisorId))
            {
                hasAccess = await _context.BusEnrollments.AnyAsync(ba => ba.BusSupervisorId == supervisorId && ba.BusId == entityId && ba.SemesterId == activeSemester.Id);
            }

            var teacherIdClaim = context.User.FindFirst(c => c.Type == "TeacherId");
            if (teacherIdClaim != null && int.TryParse(teacherIdClaim.Value, out int teacherId))
            {
                if (type == (int)AttendanceType.CLASSATTENDANCE)
                {
                    hasAccess = await _context.ClassEnrollments.AnyAsync(ce => ce.TeacherId == teacherId && ce.ClassId == entityId && ce.SemesterId == activeSemester.Id);
                }
                else if (type == (int)AttendanceType.CLUBATTENDANCE)
                {
                    hasAccess = await _context.ClubEnrollments.AnyAsync(ce => ce.TeacherId == teacherId && ce.ClubId == entityId && ce.SemesterId == activeSemester.Id);
                }
            }

            if (hasAccess)
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