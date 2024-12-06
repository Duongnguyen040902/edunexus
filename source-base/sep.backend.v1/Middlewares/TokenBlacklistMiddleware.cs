using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Services.UnitOfWork;

namespace sep.backend.v1.Middlewares
{
    public class TokenBlacklistMiddleware : TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;

        public TokenBlacklistMiddleware(RequestDelegate next, IServiceProvider serviceProvider, IConfiguration config) : base(next)
        {
            _next = next;
            _serviceProvider = serviceProvider;
            _config = config;
        }

        protected override async Task<bool> HandleRequestAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/Auth", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (context.Request.Path.StartsWithSegments(_config["VnPay:PaymentBackReturnUrl"], StringComparison.OrdinalIgnoreCase))
            {
                return true; 
            }

            if (!await base.HandleRequestAsync(context))
            {
                return false;
            }
            var refreshToken = context.Request.Cookies["refresh_token"];
            var accessToken = ExtractToken(context);

            if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(accessToken))
            {
                await WriteErrorResponse(context, 401, "Token is missing");
                return false;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var isBlacklisted = await unitOfWork.GetRepository<Blacklist>()
                    .Where(b => b.RefreshToken == refreshToken && b.AccessToken == accessToken)
                    .FirstOrDefaultAsync();

                if (isBlacklisted != null)
                {
                    await WriteErrorResponse(context, 401, "Token is blacklisted");
                    return false;
                }
            }

            return true;
        }

        private async Task WriteErrorResponse(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new { error = message };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}