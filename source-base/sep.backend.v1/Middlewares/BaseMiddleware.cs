using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Text.Json;
using sep.backend.v1.Common.Responses;

namespace sep.backend.v1.Middlewares
{
    public abstract class BaseMiddleware
    {
        private readonly RequestDelegate _next;

        protected BaseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!await HandleRequestAsync(context))
            {
                return;
            }

            await _next(context);
        }

        protected abstract Task<bool> HandleRequestAsync(HttpContext context);

        protected async Task WriteErrorResponse(HttpContext context, int statusCode, string message)
        {
            var errorResponse = new ErrorResponse(statusCode, message);
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}