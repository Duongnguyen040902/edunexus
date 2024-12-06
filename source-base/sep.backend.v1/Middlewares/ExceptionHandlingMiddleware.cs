using sep.backend.v1.Common.Responses;
using sep.backend.v1.Exceptions;
using System.Net;
using System.Text.Json;

namespace sep.backend.v1.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("The response has already started, the error handling middleware will not be executed.");
                throw exception;
            }

            context.Response.Clear();
            context.Response.ContentType = "application/json";
            var response = context.Response;

            switch (exception)
            {
                case NotFoundException ex:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case ApplicationException ex when ex.Message.Contains("Invalid Token"):
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;

                case ApplicationException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case UnauthorizedAccessException ex:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;

                case ConflictException ex:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var errorResponse = new ErrorResponse(response.StatusCode, exception.Message, new List<string> { exception.Message })
            {
                Success = false
            };

            _logger.LogError(exception, exception.Message);

            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }
    }
}