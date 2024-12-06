using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Extensions.EF;
using sep.backend.v1.Services.IServices;
using System.Threading.Tasks;

namespace sep.backend.v1.Requests.Policy
{
    public class SchoolRequirementHandler : AuthorizationHandler<SchoolRequirement>
    {
        private readonly ApplicationContext _context;

        public SchoolRequirementHandler(ApplicationContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SchoolRequirement requirement)
        {
            var schoolIdClaim = context.User.FindFirst(c => c.Type == "SchoolId");
            if (schoolIdClaim == null || !int.TryParse(schoolIdClaim.Value, out int schoolId))
            {
                context.Fail();
                return;
            }

            var teacherIdClaim = context.User.FindFirst(c => c.Type == "TeacherId");
            var pupilIdClaim = context.User.FindFirst(c => c.Type == "PupilId");
            var busSupervisorIdClaim = context.User.FindFirst(c => c.Type == "BusSupervisorId");

            bool isAuthorized = false;

            if (teacherIdClaim != null && int.TryParse(teacherIdClaim.Value, out int teacherId))
            {
                isAuthorized = await _context.Teachers.AnyAsync(t => t.Id == teacherId && t.SchoolId == schoolId);
            }
            else if (pupilIdClaim != null && int.TryParse(pupilIdClaim.Value, out int pupilId))
            {
                isAuthorized = await _context.Pupils.AnyAsync(p => p.Id == pupilId && p.SchoolId == schoolId);
            }
            else if (busSupervisorIdClaim != null && int.TryParse(busSupervisorIdClaim.Value, out int busSupervisorId))
            {
                isAuthorized = await _context.BusSupervisors.AnyAsync(b => b.Id == busSupervisorId && b.SchoolId == schoolId);
            }
            else
            {
                isAuthorized = await _context.Schools.AnyAsync(s => s.Id == schoolId);
            }

            if (isAuthorized)
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