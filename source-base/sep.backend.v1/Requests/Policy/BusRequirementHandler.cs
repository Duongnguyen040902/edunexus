using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Extensions.EF;
using System.Threading.Tasks;

namespace sep.backend.v1.Requests.Policy
{
    public class BusRequirementHandler : AuthorizationHandler<BusRequirement>
    {
        private readonly ApplicationContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BusRequirementHandler(ApplicationContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, BusRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                context.Fail();
                return;
            }

            var query = httpContext.Request.Query;
            if (!query.TryGetValue("busId", out var busIdValue) || !int.TryParse(busIdValue.ToString(), out int busId))
            {
                context.Fail();
                return;
            }

            var supervisorIdClaim = context.User.FindFirst(c => c.Type == "BusSupervisorId");
            var schoolIdClaim = context.User.FindFirst(c => c.Type == "SchoolId");
            if (supervisorIdClaim == null || !int.TryParse(supervisorIdClaim.Value, out int supervisorId) ||
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

            var busAssignmentExists = await _context.BusEnrollments.AnyAsync(ba => ba.BusSupervisorId == supervisorId && ba.BusId == busId && ba.SemesterId == activeSemester.Id);
            if (busAssignmentExists)
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