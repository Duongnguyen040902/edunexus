using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Extensions.EF;

namespace sep.backend.v1.Requests.Policy
{
    public class SchoolYearRequirementHandler : AuthorizationHandler<SchoolYearRequirement>
    {
        private readonly ApplicationContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SchoolYearRequirementHandler(ApplicationContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SchoolYearRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                context.Fail();
                return;
            }

            var query = httpContext.Request.Query;
            if (!query.TryGetValue("schoolYearId", out var schoolYearIdValue) || !int.TryParse(schoolYearIdValue.ToString(), out int schoolYearId))
            {
                context.Fail();
                return;
            }

            var userClaims = context.User.Claims;
            var schoolId = GetClaimValue(userClaims, "SchoolId");

            if (schoolId == null)
            {
                context.Fail();
                return;
            }

            var schoolYear = await _context.SchoolYears.FirstOrDefaultAsync(sy => sy.Id == schoolYearId && sy.SchoolId == schoolId);
            if (schoolYear == null)
            {
                context.Fail();
                return;
            }

            context.Succeed(requirement);
        }

        private int? GetClaimValue(IEnumerable<System.Security.Claims.Claim> claims, string claimType)
        {
            var claim = claims.FirstOrDefault(c => c.Type == claimType);
            return claim != null && int.TryParse(claim.Value, out int value) ? value : (int?)null;
        }
    }
}
