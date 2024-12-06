using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Extensions.EF;
using System.Threading.Tasks;

namespace sep.backend.v1.Requests.Policy
{
    public class ClassRequirementHandler : AuthorizationHandler<ClassRequirement>
    {
        private readonly ApplicationContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClassRequirementHandler(ApplicationContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ClassRequirement requirement)
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

            var teacherIdClaim = context.User.FindFirst(c => c.Type == "TeacherId");
            var schoolIdClaim = context.User.FindFirst(c => c.Type == "SchoolId");
            if (teacherIdClaim == null || !int.TryParse(teacherIdClaim.Value, out int teacherId) ||
                schoolIdClaim == null || !int.TryParse(schoolIdClaim.Value, out int schoolId))
            {
                context.Fail();
                return;
            }

            var activeSemester = await _context.Semesters.Include(x=>x.SchoolYear).FirstOrDefaultAsync(s => s.IsActive && s.SchoolYear.SchoolId == schoolId);
            if (activeSemester == null)
            {
                context.Fail();
                return;
            }

            var classEnrollmentExists = await _context.ClassEnrollments.AnyAsync(ce => ce.TeacherId == teacherId && ce.ClassId == classId && ce.SemesterId == activeSemester.Id);
            if (classEnrollmentExists)
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