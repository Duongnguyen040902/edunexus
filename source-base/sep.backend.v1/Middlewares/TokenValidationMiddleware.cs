using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace sep.backend.v1.Middlewares
{
    public class TokenValidationMiddleware : BaseMiddleware
    {
        public TokenValidationMiddleware(RequestDelegate next) : base(next)
        {
        }

        protected override async Task<bool> HandleRequestAsync(HttpContext context)
        {
            var token = ExtractToken(context);
            if (string.IsNullOrEmpty(token))
            {
                await WriteErrorResponse(context, 401, "Token is missing");
                return false;
            }

            return true;
        }

        protected string ExtractToken(HttpContext context, string headerName = "Authorization")
        {
            if (context.Request.Headers.TryGetValue(headerName, out var headerValue))
            {
                return headerValue.ToString().Split(" ").Last();
            }
            return null;
        }
    }
}