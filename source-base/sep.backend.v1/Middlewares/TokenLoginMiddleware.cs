using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace sep.backend.v1.Middlewares
{
    public class TokenLoginMiddleware : TokenValidationMiddleware
    {
        private readonly IConfiguration _config;
        public TokenLoginMiddleware(RequestDelegate next, IConfiguration config) : base(next)
        {
            _config = config;
        }

        protected override async Task<bool> HandleRequestAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/Auth", StringComparison.OrdinalIgnoreCase) ||
                context.Request.Path.StartsWithSegments(_config["VnPay:PaymentBackReturnUrl"], StringComparison.OrdinalIgnoreCase) ||
                context.Request.Path.StartsWithSegments("/Resources", StringComparison.OrdinalIgnoreCase) ) 
            {
                return true;
            }

            if (!await base.HandleRequestAsync(context))
            {
                return false;
            }

            var token = ExtractToken(context);
            var jwtHandler = new JwtSecurityTokenHandler();

            if (!jwtHandler.CanReadToken(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid token");
                return false;
            }

            var jwtToken = jwtHandler.ReadJwtToken(token);
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var role = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var schoolId = jwtToken.Claims.FirstOrDefault(c => c.Type == "SchoolId")?.Value;
            var schoolAdminId = jwtToken.Claims.FirstOrDefault(c => c.Type == "SchoolAdminId")?.Value;
            var pupilId = jwtToken.Claims.FirstOrDefault(c => c.Type == "PupilId")?.Value;
            var teacherId = jwtToken.Claims.FirstOrDefault(c => c.Type == "TeacherId")?.Value;
            var busSupervisorId = jwtToken.Claims.FirstOrDefault(c => c.Type == "BusSupervisorId")?.Value;
            if (!string.IsNullOrEmpty(email)) context.Items["UserEmail"] = email;
            if (!string.IsNullOrEmpty(role)) context.Items["UserRole"] = role;
            if (!string.IsNullOrEmpty(schoolId)) context.Items["SchoolId"] = schoolId;
            if (!string.IsNullOrEmpty(schoolAdminId)) context.Items["SchoolAdminId"] = schoolAdminId;
            if (!string.IsNullOrEmpty(pupilId)) context.Items["PupilId"] = pupilId;
            if (!string.IsNullOrEmpty(teacherId)) context.Items["TeacherId"] = teacherId;
            if (!string.IsNullOrEmpty(busSupervisorId)) context.Items["BusSupervisorId"] = busSupervisorId;

            return true;
        }
    }
}
